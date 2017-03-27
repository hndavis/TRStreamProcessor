using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Xml.Schema;
using TRStreamProcessor.Msg;
using TRStreamProcessor.Service;
namespace WCFTestClient
{
    class Program
    {
        public static INotificationServices Proxy
        {
            get
            {
                try
                {
                    var ctx = new InstanceContext(new NotificationServiceCallBack());
                    return new DuplexChannelFactory<INotificationServices>
                        (ctx, "WSDualHttpBinding_INotificationServices").CreateChannel();
                }
                catch (Exception x)
                {
                    Console.WriteLine(x);
                    return null;
                }
            }
        }
        static void Main(string[] args)
        {
           var proxy = Proxy;
            proxy?.SendNotification("Are u ready?");
            Console.ReadLine();
        }

        static void TestWCFSimplex()
        {
            try
            {
                StreamOutServiceRef1.StreamOutServiceClient clientRef =
                    new StreamOutServiceRef1.StreamOutServiceClient();
                var qName = clientRef.GetMessageQueueName();
                Console.WriteLine("QueName = {0}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
            }
            //http://stackoverflow.com/questions/2943148/how-to-programmatically-connect-a-client-to-a-wcf-service
            var myBinding = new BasicHttpBinding();
            var myEndpoint = new EndpointAddress("http://localhost:8080/StreamOutService");
            var myChannelFactory = new ChannelFactory<IStreamOutService>(myBinding, myEndpoint);

            IStreamOutService client = null;

            try
            {
                client = myChannelFactory.CreateChannel();
                var qName = client.GetMessageQueueName();
                Console.WriteLine("QueName = {0}");
                ((ICommunicationObject)client).Close();
            }
            catch (Exception ex2)
            {
                Console.WriteLine("Exception: {0}", ex2);
                if (client != null)
                {
                    ((ICommunicationObject)client).Abort();
                }
            }
        }

    }
}

