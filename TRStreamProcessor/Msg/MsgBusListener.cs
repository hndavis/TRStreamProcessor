using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Consumer;
using TRStreamProcessor.Data;

namespace TRStreamProcessor.Msg
{
    public class MsgBusListener
    {
        private IBus Bus;
        private Action<TrsTupple> OnReceive;
        private ISubscriptionResult HelloSubResult;
        public MsgBusListener(IBus bus, Action<TrsTupple> onReceive)
        {
            Bus = bus;
            OnReceive = onReceive;
        }

        public void Start()
        {
            HelloSubResult = Bus.Subscribe<Hello>("sub_id", msg => Console.WriteLine(msg.Text));
        }

        public void Stop()
        {
            HelloSubResult.Dispose();
        }
    }
}
