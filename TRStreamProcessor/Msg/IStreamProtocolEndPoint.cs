using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRStreamProcessor.Stream;
using TRStreamProcessor.Data;

namespace TRStreamProcessor.Msg
{
   public  enum ProtocolType
    {
        WCFBin,
        WCFBasicHttp,
        RabbitMQ,
        MSMQ                //supported only for dev
    }

    public enum FlowType
    {
        OneWay,
        TwoWay
    }

    public enum Direction
    {
        In,
        Out
    }

    public interface IStreamProtocolEndPoint
    {
       void Connect();
        void SetEndPointInfo(string s); //name  , connection info ?
        ProtocolType ProtocolType { get; set; }
        Direction Direction { get; set; }
        void ToTrsTuppleAction(Action a);
        TrsTupple ToTrsTupple();



    }
}
