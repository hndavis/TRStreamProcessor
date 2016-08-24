using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using TRStreamProcessor.Msg;

namespace TrsTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var localBus = RabbitHutch.CreateBus("host=localhost");
            var helloMsg = new Hello();
            helloMsg.Text = "FirstMsg";
            localBus.Publish(helloMsg);
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }
    }
}
