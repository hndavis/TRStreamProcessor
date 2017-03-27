using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace TRStreamProcessor.Service
{
    
    public interface INotificationServiceCallBack
    {
        [OperationContract(IsOneWay = true)]
        void OnNotificationSend(string message);
    }
}
