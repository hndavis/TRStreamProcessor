using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TRStreamProcessor.Service
{
    [ServiceContract(CallbackContract = typeof(INotificationServiceCallBack))]
    interface INotificationServices
    {
        [OperationContract]
        void SendNotification(string message);
    }
}
