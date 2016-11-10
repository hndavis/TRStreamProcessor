using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace TrsStreamsHost
{
    [ServiceContract]
    public interface IPublishing
    {
        [OperationContract(IsOneWay = true)]
        void Publish(Message e, string topicName);
    }
}
