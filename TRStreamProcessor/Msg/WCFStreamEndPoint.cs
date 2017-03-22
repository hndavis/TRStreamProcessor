using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRStreamProcessor.Data;

namespace TRStreamProcessor.Msg
{
    public class WCFStreamEndPoint : IStreamProtocolEndPoint
    {
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

        public Direction Direction{ get; set;}
    }
}
