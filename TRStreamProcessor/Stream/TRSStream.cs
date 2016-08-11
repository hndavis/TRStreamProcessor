using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Threading;
using TRStreamProcessor.Data;

namespace TRStreamProcessor.Stream
{
    public class TrsStream <TTuppleIn, TTuppleOut>  
        where TTuppleIn: TrsTupple, new()
        where TTuppleOut: TrsTupple, new()
    {
        Guid id = new Guid();
        IObservable<TrsTupple> InStream;
        private IDisposable InObserver;
        private IObserver<TTuppleOut> outStream;

        //  TRSTupples to listens to
        public void SetInStream(IObservable<TTuppleIn> inStream)
        {
            InStream = inStream;
            InObserver = Subscribe();
        }

        public IDisposable Subscribe()
        {
            return InStream.Subscribe();
        }

        public bool pub(TTuppleOut tupple)
        {
            if (outStream != null)
            {
                outStream.OnNext(tupple);
                return true;
            }
            else
            {
                return false;
            }
        }



        // list of stream to publish to.
        public IObservable<TTuppleOut> GetOutStream()
        {
            return Observable.Create<TTuppleOut>(
                (IObserver<TTuppleOut> observer) =>
                {
                    outStream = observer;
                   // observer.OnNext(new TTuppleOut());
                   // observer.OnCompleted();
                   // Thread.Sleep(1000);
                    return Disposable.Create(() => Console.WriteLine("Observer has unsubscribed"));
                }
                );
        }
    }
}
