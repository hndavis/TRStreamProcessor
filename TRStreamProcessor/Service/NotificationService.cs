using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TRStreamProcessor.Service
{
    public class NotificationService : INotificationServices
    {
        public INotificationServiceCallBack Proxy
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<INotificationServiceCallBack>();
            }
        }

        public void SendNotification(string message)
        {
            Console.WriteLine("\nClient says :" + message);
            var proxy = Proxy;
            proxy.OnNotificationSend("Yes");
            var r = new Random(10);
            for (int i = 0; i < 100; i++)
            {
                //Thread.Sleep(r.Next());
                proxy.OnNotificationSend(i.ToString());
                
            }
        }
    }
}
