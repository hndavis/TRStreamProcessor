using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TRStreamProcessor.TPL
{
    


    public sealed class TrsScheduler : TaskScheduler, IDisposable
    {
        private readonly BlockingCollection<Task> Tasks = new BlockingCollection<Task>();
        private readonly Thread MainThread ;

        public TrsScheduler()
        {
            MainThread = new Thread(Main);
        }

        private void Main()
        {
            Console.WriteLine("Starting Thread " + Thread.CurrentThread.ManagedThreadId.ToString());

            foreach (var t in Tasks.GetConsumingEnumerable())
            {
                TryExecuteTask(t);
            }
        }

        /// <summary>
        /// Used by the Debugger
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return Tasks.ToArray<Task>();
        }


        protected override void QueueTask(Task task)
        {
            Tasks.Add(task);

            if (!MainThread.IsAlive) { MainThread.Start(); }//Start thread if not done so already
        }


        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }


        #region IDisposable Members

        public void Dispose()
        {
            Tasks.CompleteAdding(); //Drops you out of the thread
        }

        #endregion

      
    }
}
