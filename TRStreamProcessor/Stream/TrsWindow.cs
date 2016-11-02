using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRStreamProcessor.Data;
using TRStreamProcessor.TPL;

namespace TRStreamProcessor.Stream
{
    public enum TrsWindowType
    {
        MaxNum, MaxTime
    }

    public  enum TrsGroupOperations
    {  Sum, Avg, Count}

    /* this class hold infomration about current stream info  over a timed window or counted items window 
     * two flows ( threads )
     * one where a new item is inserted into a window
     * one where a old item leaves 
     * 
     * it also listens to a TrsStream for activity
     */

    //http://stackoverflow.com/questions/17020011/iobserver-and-iobservable-in-c-sharp-for-observer-vs-delegates-events
    public class TrsWindow<TObservable, TObserver,TTuppleIn, TTupleOut> : TrsStream<TObservable, TObserver, TTuppleIn, TTupleOut >, 
        IStreamListener, IActiveWindow
        where TTuppleIn : TrsTupple, new()
        where TTupleOut : TrsTupple, new()
         where TObservable : IObservable<TTuppleIn>
        where TObserver : IObserver<TTupleOut>



    {
        private readonly ConcurrentQueue<TrsTuppleQWraper<TTuppleIn>> ValuesInProgress = new ConcurrentQueue<TrsTuppleQWraper<TTuppleIn>>();
        private readonly ConcurrentDictionary<string, Row> CurrWindowValues = new ConcurrentDictionary<string, Row>();
        public readonly  Guid Guid = Guid.NewGuid();
        private readonly TimeSpan WindowDelay ;//= TimeSpan.FromSeconds(60);
        private readonly List<TrsString> GroupByColumns;
        private readonly Dictionary<string, TrsLong> ComputedColumns = new Dictionary<string,TrsLong>();
        private readonly TaskScheduler InCommingScheduler;
        private readonly TaskScheduler OutGoingScheduler;
        private readonly TrsTuppleFactory OutDataPointFactory;

        private List<IObserver<TrsTupple>> observers;

    
        TrsString KeyValueDef;

       private readonly  CancellationTokenSource  Cts = new CancellationTokenSource();

        

        public TrsWindow(String name, TObservable obs, 
           
            List<TrsString> groubyColumns, List<TrsLong> computedColumns,
            int amount = -1, TrsWindowType type = TrsWindowType.MaxTime,
            TaskScheduler inCommingScheduler = null,
            TaskScheduler outGoingScheduler = null) : base (name,obs)

        {
            Obs = obs;
            GroupByColumns = groubyColumns;
            foreach (var col in computedColumns)
            {
                ComputedColumns[col.Name] = col;
            }

            InCommingScheduler = inCommingScheduler ?? TaskScheduler.Current;
            OutGoingScheduler = outGoingScheduler ?? TaskScheduler.Current;
            if ( amount == -1)
                 WindowDelay = TimeSpan.FromSeconds(60);
            else
            {
                if ( amount < 1 )
                    throw new Exception("Amount must be greater than 1");
                WindowDelay = TimeSpan.FromSeconds(amount);
            }

           

            KeyValueDef = new TrsString("keyValue") { Name = "RowKey" };

            TrsLong dataPointSumDef = new TrsLong(0) { Name = "Sum" };

            var defDataPointTupple = new TrsTuppleDef(KeyValueDef, dataPointSumDef);
            OutDataPointFactory = new TrsTuppleFactory(defDataPointTupple);
            //if ( observer != null)
            //     Subscribe(observer);
        }
        public IDisposable Subscribe(IObserver<TrsTupple> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<TrsTupple>> _observers;
            private IObserver<TrsTupple> _observer;

            public Unsubscriber(List<IObserver<TrsTupple>> observers, IObserver<TrsTupple> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        public void EndTransmission()
        {
            foreach (var observer in observers.ToArray())
                if (observers.Contains(observer))
                    observer.OnCompleted();

            observers.Clear();
        }

        public void SendMessage(TrsTupple trsTupple)
        {
            foreach (var observer in observers)
            {
               
                    observer.OnNext(trsTupple);
            }
        }

        public class MessageUnknownException : Exception
        {
            internal MessageUnknownException()
            {
            }
        }

        IDisposable MsgHandle;
        private readonly IObservable<TTuppleIn> Obs;

        public void Dispose()
        {
            MsgHandle.Dispose();
        }

        public void Listen()
        {
            Start();  // this windows processing incomming tupples

            MsgHandle = Obs.Subscribe(t =>
            {
//                Console.WriteLine("{0}  Received On {1} --> {2} ", Name, t.ToString(), Thread.CurrentThread.ManagedThreadId);
                HandleIncomingTupples(t);
            }
                );
        }
        protected override IObservable<TTupleOut> setUpOutStream()
        {
           
            return (IObservable<TTupleOut>)Observable.Create<TTupleOut>(
                    observer =>
                    {
                        OutStream = (TObserver)observer;

                        return Disposable.Create(() => Console.WriteLine("Observer has unsubscribed"));
                    }
                   );
        }

        

        void OnRowUpdated(string key)
        {
            Debug.WriteLine("{0}: Recomputed   {1}:{2}" ,Name, key, CurrWindowValues[key].ComputedColumns[0].sum);
          
            var keyValue = new TrsString(key) { Name = "RowKey" };
            
            var dataPointSum = new TrsLong((int)CurrWindowValues[key].ComputedColumns[0].sum) { Name = "Sum" };


            if (OutStream != null)
            {
                var tOut = OutDataPointFactory.Create(keyValue, dataPointSum);
                if (tOut != null)
                    OutStream.OnNext((TTupleOut) tOut); //todo   should not need cast -- find other way

              
            }

           

        }

    
      
        void HandleIncomingTupples(TTuppleIn t)
        {
            // put into queue for later removeall based on time
            ValuesInProgress.Enqueue(new TrsTuppleQWraper<TTuppleIn>(t, WindowDelay));

            //put into actual window structure based on key

            //if there a existing row ?
            CurrWindowValues.AddOrUpdate(RowKey(t),
                (k) =>
                {
                    Row r = new Row {GroupedFields = new List<ITrsTypedField>()};
                    foreach (var col in GroupByColumns  )
                    {
                        TrsString colString = new TrsString();
                        colString.Val = t.Fields[col.Name].ToString();
                        r.GroupedFields.Add(colString);

                    }
                    r.ComputedColumns = new List<ColumnStatistic>();
                    foreach (var computedColumn in ComputedColumns.Values)
                    {

                        ColumnStatistic colStat = new ColumnStatistic
                        {
                            count = 1,
                            underlyingName = computedColumn.Name
                        };
                        switch (computedColumn.ttype)
                        {
                                case TrsType.Flt:
                               
                                colStat.avg = (int)t.Fields[computedColumn.Name].RawVal;
                                colStat.sum = colStat.avg;
                                break;

                                case TrsType.Integer:
                                colStat.avg = (int)t.Fields[computedColumn.Name].RawVal;
                                colStat.sum = colStat.avg;
                                break;

                            case TrsType.Long:
                                colStat.avg = (long)t.Fields[computedColumn.Name].RawVal;
                                colStat.sum = colStat.avg;
                                break;

                            default:
                                throw new Exception("Type not supported for compute.");
                        }

                      
                        r.ComputedColumns.Add(colStat);
                    }
                    r.InsertTime = DateTime.Now;
                  
                    return  r;
                },
                (k, row) =>
                {

                    foreach (var colStat in row.ComputedColumns)
                    {
                        colStat.count ++;
                        switch (t.Fields[colStat.underlyingName].ttype)
                        {
                            case TrsType.Flt:

                                colStat.sum += (int) t.Fields[colStat.underlyingName].RawVal;
                                colStat.avg = colStat.sum/colStat.count;
                                break;

                            case TrsType.Integer:
                                colStat.sum += (int)t.Fields[colStat.underlyingName].RawVal;
                                colStat.avg = colStat.sum / colStat.count;
                                break;

                            case TrsType.Long:
                                colStat.sum += (long)t.Fields[colStat.underlyingName].RawVal;
                                colStat.avg = colStat.sum / colStat.count;
                                break;

                            default:
                                throw new Exception("Type not supported for compute.");


                        }
                      
                    }
                  
                    return row;
                })
            ;
            OnRowUpdated(RowKey(t));
        }
        
        
        private string RowKey(TTuppleIn t)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var fldname in GroupByColumns)
            {
                sb.Append((string)t.Fields[fldname.Name].RawVal);

            }
            return sb.ToString();
        }

        void HandleOutGoingTupple(TTuppleIn t)
        {
            CurrWindowValues.AddOrUpdate(RowKey(t), (k) =>
            {
                throw new Exception("Cannot Expire Row  " + k);
            },
            (k, row) =>
            {

                foreach (var colStat in row.ComputedColumns)
                {
                    colStat.count++;
                    switch (ComputedColumns[colStat.underlyingName].ttype)
                    {
                        case TrsType.Flt:

                            colStat.sum -= (double)t.Fields[colStat.underlyingName].RawVal;
                            colStat.avg = colStat.sum / colStat.count;
                            break;

                        case TrsType.Integer:
                            colStat.sum -= (int)t.Fields[colStat.underlyingName].RawVal;
                            colStat.avg = colStat.sum / colStat.count;
                            break;

                        case TrsType.Long:
                            colStat.sum -= (long)t.Fields[colStat.underlyingName].RawVal;
                            colStat.avg = colStat.sum / colStat.count;
                            break;

                        default:
                            throw new Exception("Type not supported for compute.");


                    }

                }
               
                return row;
                   });
            OnRowUpdated(RowKey(t));
        }
        
        private class ColumnStatistic
        {
            public string underlyingName { get; set; }
            public int count { get; set; }
            public double sum { get; set; }
            public double avg { get; set; }
        }

        private class Row
        {
            public List<ITrsTypedField> GroupedFields;

            public List<ColumnStatistic> ComputedColumns;
            public DateTime InsertTime;

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                
                foreach (var col in GroupedFields)
                {
                    sb.Append((string) col.RawVal+ "\t");
                }
                foreach (var computedCol in ComputedColumns)
                {
                    sb.Append("Avg:" + computedCol.avg + " ");
                    sb.Append("Sum:" + computedCol.sum + " ");
                }
               
                return base.ToString();
            }
        }

        private class TrsTuppleQWraper <TIn> where TIn : TrsTupple, new()
        {
            public readonly TIn TrsTuple;
            public readonly DateTime InsertTime;

            public TrsTuppleQWraper(TIn t, TimeSpan offset)
            {
                TrsTuple = t;
                InsertTime = DateTime.Now + offset;
                
            }
            
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Window: " + Guid);
            sb.Append("\n Cols ");
            foreach (var groupByColumn in GroupByColumns)
            {
                sb.Append("\t " + groupByColumn.Name);
            }
            sb.Append("\n Compute Cols ");
            foreach ( var computedVal in ComputedColumns.Values )
            {
                sb.Append("\t "+ computedVal.Name);
              
                
            }

            return sb.ToString();
        }

        public void Start()
        {

            //to do find way to control number of threads started
           
            var t = new Task( () => ProcessExperiation(Cts.Token), Cts.Token);
            t.Start(OutGoingScheduler);
        }

        void ProcessExperiation(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                // peek from queue
                TrsTuppleQWraper<TTuppleIn> trsTimedTupple;
                while (ValuesInProgress.Count == 0)
                    Thread.Sleep(100);
                ValuesInProgress.TryPeek(out trsTimedTupple);
                // wait for expiry
                while (trsTimedTupple.InsertTime > DateTime.Now)
                {
                    Thread.Sleep(trsTimedTupple.InsertTime - DateTime.Now);
                }
                // if expired deque item

                ValuesInProgress.TryDequeue(out trsTimedTupple);
                // remove from current window
                if (trsTimedTupple != null)
                {
                    HandleOutGoingTupple(trsTimedTupple.TrsTuple);

                }

            }
        }

        public void Stop()
        {
            Cts.Cancel();
        }

       

    }
}
