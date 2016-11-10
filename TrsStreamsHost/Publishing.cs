using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace TrsStreamsHost
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    class Publishing : IPublishing
    {
        #region IPublishing Members
        public void Publish(Message e, string topicName)
        {
            List<IPublishing> subscribers = Filter.GetSubscribers(topicName);

            Type type = typeof(IPublishing);
            MethodInfo publishMethodInfo = type.GetMethod("Publish");
            foreach (IPublishing subscriber in subscribers)
            {
                try
                {
                    publishMethodInfo.Invoke(subscriber, new object[] { e, topicName });
                }
                catch
                {

                }

            }


        }


        #endregion
    }
}
