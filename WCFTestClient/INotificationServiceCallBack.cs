using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFTestClient
{
    public interface INotificationServiceCallBack
    {
        [OperationContract(IsOneWay = true)]
        void OnNotificationSend(string message);
        

    }
}
