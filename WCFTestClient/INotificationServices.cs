using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using TRStreamProcessor.Msg;
using TRStreamProcessor.Service;

namespace WCFTestClient
{
    [ServiceContract(CallbackContract = typeof(INotificationServiceCallBack))]
    interface INotificationServices
    {
        [OperationContract]
        void SendNotification(string message);
    }
}
