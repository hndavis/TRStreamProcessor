using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using TRStreamProcessor.Data;

namespace TRStreamProcessor.Msg
{
    public class WCFHttpStreamEndPoint : IStreamProtocolEndPoint
    {
        public string EndPointAddress { get; set; }
        public ServiceHost host { get; set; }

        public Direction Direction   { get; set;   }

        public ProtocolType ProtocolType
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void SetEndPointInfo(string s)
        {
            throw new NotImplementedException();
        }

        public TrsTupple ToTrsTupple()
        {
            throw new NotImplementedException();
        }

        public void ToTrsTuppleAction(Action a)
        {
            throw new NotImplementedException();
        }
    }
}
