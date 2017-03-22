using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using TRStreamProcessor.Service;

namespace TRStreamProcessor.Msg
{
    public static class ProtocolEndPointFactory
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProtocolEndPointFactory));
        public static IStreamProtocolEndPoint GetEndPoint(ProtocolType pt)
        {
            IStreamProtocolEndPoint hStreamEndPoint = null;
            switch (pt)
            {
                case ProtocolType.MSMQ:
                    return null;
                 
                case ProtocolType.RabbitMQ:
                    return null;
                case ProtocolType.WCFBin:
                    return null;

                case ProtocolType.WCFBasicHttp:
                    
                    //System.Net.Dns.GetHostEntry((System.Net.Dns.GetHostName(), Port);
                    // var sh = new System.ServiceModel.ServiceHost ( GetType(ServiceSecurityContext.))
                    log.Info("Creating a service host for WCF...");
                    hStreamEndPoint = new WCFHttpStreamEndPoint();
                    var sh = new ServiceHost(typeof(StreamOutService));
                    ((WCFHttpStreamEndPoint)hStreamEndPoint).host = sh;
                    sh.Open();
                    log.Info(string.Format("address {0}", sh.BaseAddresses[0].AbsoluteUri));
                   
                    return null;

                default:
                    return null;

            }
        }
    }
}
