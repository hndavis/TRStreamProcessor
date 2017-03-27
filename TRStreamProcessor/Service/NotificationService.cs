using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
            Proxy.OnNotificationSend("Yes");
        }
    }
}
