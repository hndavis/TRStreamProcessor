using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFTestClient
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class NotificationServiceCallBack : INotificationServiceCallBack
    {
    
        public void OnNotificationSend(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
