using System;
using System.Threading;
using TRStreamProcessor.Data;

namespace TRStreamProcessor.Stream
{
    public class TrsListener : IDisposable
    {
        public TrsListener(IObservable<TrsTupple> obs)
        {
            Obs = obs;
            Listen();
        }

       IDisposable msgHandle;
        private IObservable<TrsTupple> Obs;

        public void Dispose()
        {
            msgHandle.Dispose();
        }

        public void Listen()
        {

            Console.WriteLine("Press any to to stop listening");

            msgHandle = Obs.Subscribe(t =>
            {
                Console.WriteLine("Received: On {0} " + t.ToString(), Thread.CurrentThread.ManagedThreadId);
            }
          );
            //Console.ReadKey();
            //Dispose();
        }
    }
}
