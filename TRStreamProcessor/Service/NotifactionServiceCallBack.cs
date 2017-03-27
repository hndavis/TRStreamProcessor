using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace TRStreamProcessor.Service
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class NotifactionServiceCallBack : INotificationServiceCallBack
    {
                    
        public void OnNotificationSend(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
