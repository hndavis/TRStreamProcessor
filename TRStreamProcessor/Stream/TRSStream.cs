using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using TRStreamProcessor.Data;

namespace TRStreamProcessor.Stream
{
    public class TrsStream<TObservable, TObserver, TTupleIn, TTupleOut> :
        IObservable<TTupleOut>
        where TTupleIn : TrsTupple, new()
        where TTupleOut : TrsTupple, new()
        where TObservable : IObservable<TTupleIn>
        where TObserver : IObserver<TTupleOut>
    {
        public Guid Guid { get; } = new Guid();
        public String Name {  get; private set; }

        private   TObservable InObservable;
       // protected TObserver OutStream;  //todo shoud be  private IObserver<TTuppleOut> OutStream;
        protected readonly List<IObserver<TTupleOut>> _observers = new List<IObserver<TTupleOut>>();

        private readonly Func<TTupleIn, TTupleOut> Transform;

        //  TRSTupples to listens to
        public TrsStream(String name,TObservable inObservable)
        {
            Transform = defaultTransform;
            SubscribeTo(inObservable);
            Name = name;
        }

        public TObservable GetSourceObservable()
        {
            return InObservable;
        }

        
        TTupleOut defaultTransform(TTupleIn tIn)
        {
            return tIn as TTupleOut;
        }

       
        
        public IDisposable Subscribe(IObserver<TTupleOut> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
           
        }

        public IDisposable SubscribeTo(TObservable inObservable)
        {
            if (inObservable == null)
                return null;

            InObservable = inObservable;
            if (InObservable != null)
                return InObservable.Subscribe(process);

            return null; // could be first point in a stream/flow
        }

        /// <summary>
        /// Like EnQue -- but for streams
        /// </summary>
        /// <param name="tin"></param>

        public virtual void EnStream(TTupleIn tin)
        {
            process( tin);
        }

        public virtual void process(TTupleIn tin)
        {
            var t = Transform(tin);
            var currentObs = _observers.ToImmutableList();
            foreach (var obs in currentObs)
            {
                obs.OnNext(t);
            }
        }
      
        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<TTupleOut>> _observers;
            private readonly IObserver<TTupleOut> _observer;

            public Unsubscriber(List<IObserver<TTupleOut>> observers, IObserver<TTupleOut> observer)
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


    }
    
}
