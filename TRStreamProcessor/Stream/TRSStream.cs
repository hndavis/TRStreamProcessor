using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Threading;
using TRStreamProcessor.Data;

namespace TRStreamProcessor.Stream
{
    public class TrsStream<TObservable, TObserver, TTupleIn, TTupleOut> : IDisposable, IObservable<TTupleOut>
        where TTupleIn : TrsTupple, new()
        where TTupleOut : TrsTupple, new()
        where TObservable : IObservable<TTupleIn>
        where TObserver : IObserver<TTupleOut>


    {
        private readonly Guid id = new Guid();

        public Guid Guid
        {
            get { return id; }
        }
        public String Name {  get; private set; }


        private IDisposable InternalObserver;
        private readonly TObservable InObservable;
        protected TObserver OutStream;  //todo shoud be  private IObserver<TTuppleOut> OutStream;
        private IObservable<TTupleOut> OutObservable;

        private readonly Func<TTupleIn, TTupleOut> Transform;

        //  TRSTupples to listens to
        public TrsStream(String name,TObservable inObservable)
        {
            Transform = defaultTransform;
            OutObservable = setUpOutStream();
            InObservable = inObservable;
            InternalObserver = Subscribe();
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            //may not need
            Dispose(true);
        }

        protected void Dispose(bool bDispose)
        {
            OutObservable = null;
            InternalObserver = null;
        }

        

        TTupleOut defaultTransform(TTupleIn tIn)
        {
            return tIn as TTupleOut;
        }

        public TObserver GetOutStream()
        {
            return OutStream ;
        }

        public IObservable<TTupleOut> GetObservable()
        {
            return OutObservable;
        }

        public IDisposable Subscribe(IObserver<TTupleOut> observer)
        {
            return OutObservable.Subscribe(observer);
        }

        public IDisposable Subscribe()
        {
            if (InObservable != null)
                return InObservable.Subscribe(t =>
                {
                //    Console.WriteLine("{0}: In Subscribe :" + t.ToString(),Name);
                    process(Transform(t));
                });
            else
            {
                //throw new Exception("In Stream not set");
                return null;  /// could be first point in a stream/flow
            }
        }

        public bool process(TTupleOut t)
        {
//            Console.WriteLine("{0}: In process :" + t.ToString(), Name);
            if (OutStream != null)
            {
                OutStream.OnNext(t);
                return true;
            }
            else
            {
                return false;
            }
        }



        // list of stream to publish to.
         protected virtual IObservable<TTupleOut> setUpOutStream()
        {
           return (IObservable<TTupleOut>) Observable.Create<TTupleOut>(
                 observer =>
                {
                    OutStream = (TObserver)observer;
                   
                    return Disposable.Create(() => Console.WriteLine("Observer has unsubscribed"));
                }
                );
          
        }

       
    }
}
