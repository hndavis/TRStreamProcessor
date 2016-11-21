using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using TRStreamProcessor.Data;
using TRStreamProcessor.Msg;
using TRStreamProcessor.Stream;

namespace TrsTestClient
{
    class Program
    {
        static void Main(string[] args)
        {





            var entryPoint = new TrsStream<IObservable<TrsTupple>, IObserver<TrsTupple>, TrsTupple, TrsTupple>("EntryPoint", null); // null means does not listen to any outside stream

#if RABBIT
            //test rabbit
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = "amqp://guest:guest@localhost:5672/vhost";
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.ExchangeDeclare("TestEX", ExchangeType.Direct);
            channel.QueueDeclare("testQ", false, false, false, null);
            channel.QueueBind("testQ","TestEx","routekey");


             var testBus = RabbitHutch.CreateBus("host=localhost");        
            var busListener = new MsgBusListener(testBus, (m) =>
            {
                entryPoint.process(m);
            });
            Thread.Sleep(50000);
            return;
#endif
            string col1 = "TestAppId";
            string col2 = "TestUserId";
            string col3 = "NumDataPoints";
            var applicationIdDef = new TrsString("TestApp") { Name = col1 };
            var userNameDef = new TrsString("TestUser") { Name = col2 };
            var dataPointsDef = new TrsLong(0) { Name = col3 };

            //var listenTask = Task.Factory.StartNew(() =>
            //        {
            //            var listener = new TrsListener(entryPoint.GetOutStream());
            //           // listener.Listen();

            //        }
            //    );
            //TrsStream<IObservable<TrsTupple>, IObserver<TrsTupple>, TrsTupple, TrsTupple> sampleStream;
            //var streamTestTask = Task.Factory.StartNew(() =>
            //        {
            //            sampleStream = new TrsStream<   IObservable<TrsTupple>, IObserver< TrsTupple >,TrsTupple, TrsTupple >
            //              ( "streamTesk", entryPoint.GetObservable());

            //        });
            //streamTestTask.Wait();
            var groupbyColumns = new List<TrsString> { applicationIdDef, userNameDef };
            var computeColumns = new List<TrsLong> { dataPointsDef };
            var winTask = Task.Factory.StartNew(() =>
            {
                var simpleWindowSum = new TrsWindow<IObservable<TrsTupple>, IObserver<TrsTupple>, TrsTupple, TrsTupple>
                ("BASIC MATH", entryPoint,
                    groupbyColumns, computeColumns, 60, TrsWindowType.MaxNum);
                simpleWindowSum.Subscribe(item =>
                {
                    Console.WriteLine("1 Processed Item {0} {1} ", item.Fields["RowKey"], item.Fields["Sum"]);
                });
                simpleWindowSum.Listen();
                //while (true)
                //{
                //    Thread.Sleep(1000 * 10);
                //    Console.WriteLine("1\t\t\t\t\t Agg:");
                //    Console.Write("1\t\t\t\t\t");
                //    Console.WriteLine(" Counts {0} ==> {1}", simpleWindowSum.CountDetail(), simpleWindowSum.CountAggregated() );
                //    var topRows = simpleWindowSum.Top(1);
                //    Console.WriteLine("1\t\t\t\t\t\t Top:");
                //    foreach (var topRow in topRows)
                //    {
                //        Console.WriteLine("1\t\t\t\t\t\t {0} {1} {2}", topRow.Item1, topRow.Item2, simpleWindowSum.CountDetail(topRow.Item1));
                //    }

                //   Console.WriteLine();
                //}
            });

            var winTask2 = Task.Factory.StartNew(() =>
            {
                var simpleWindowSum2 = new TrsWindow<IObservable<TrsTupple>, IObserver<TrsTupple>, TrsTupple, TrsTupple>
                ("BASIC MATH", entryPoint,
                    groupbyColumns, computeColumns, 60, TrsWindowType.MaxNum);
                simpleWindowSum2.Subscribe(item =>
                {
                    Console.WriteLine("2 Processed Item {0} {1} ", item.Fields["RowKey"], item.Fields["Sum"]);
                });
                simpleWindowSum2.Listen();
                //while (true)
                //{
                //    Thread.Sleep(1000 * 10);
                //    Console.WriteLine("2\t\t\t\t\t Agg:");
                //    Console.Write("2\t\t\t\t\t");
                //    Console.WriteLine(" Counts {0} ==> {1}", simpleWindowSum2.CountDetail(), simpleWindowSum2.CountAggregated());
                //    var topRows = simpleWindowSum2.Top(1);
                //    Console.WriteLine("2\t\t\t\t\t\t Top:");
                //    foreach (var topRow in topRows)
                //    {
                //        Console.WriteLine("2\t\t\t\t\t\t {0} {1} {2}", topRow.Item1, topRow.Item2, simpleWindowSum2.CountDetail(topRow.Item1));
                //    }

                //    Console.WriteLine();
                //}
            });

            // winTask.Wait(); // make sure subscribed

            var defDataPointTupple = new TrsTuppleDef(applicationIdDef, userNameDef, dataPointsDef);
            TrsTuppleFactory applDataPointFactory = new TrsTuppleFactory(defDataPointTupple);

            var rd = new Random();



            var taskAddData2 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    var applicationId = new TrsString("TestAppId") { Name = col1 };
                    var userName = new TrsString("TestUserIdTwo") { Name = col2 };
                    // var dataPoints = new TrsInt(rd.Next()) { Name = col3 };
                    var dataPoints = new TrsLong(3) { Name = col3 };

                    //var sampleTuple = new TrsTupple("Sample"+i.ToString(),
                    //   (TrsTypedField)applicationId, userName, dataPoints);

                    var sampleTuple = applDataPointFactory.Create(applicationId, userName, dataPoints);

                    entryPoint.EnStream(sampleTuple);
                    Thread.Sleep(1400);

                }
            });


            var taskAddData3 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    var applicationId = new TrsString("TestAppIdTwo") { Name = col1 };
                    var userName = new TrsString("TestUserIdThree") { Name = col2 };
                    // var dataPoints = new TrsInt(rd.Next()) { Name = col3 };
                    var dataPoints = new TrsLong(3) { Name = col3 };

                    //var sampleTuple = new TrsTupple("Sample"+i.ToString(),
                    //   (TrsTypedField)applicationId, userName, dataPoints);

                    var sampleTuple = applDataPointFactory.Create(applicationId, userName, dataPoints);

                    entryPoint.EnStream(sampleTuple);
                    Thread.Sleep(4500);

                }
            });
            Thread.Sleep(200000);
            // winTask.Dispose();

            //var localBus = RabbitHutch.CreateBus("host=localhost");
            //var helloMsg = new Hello();
            //helloMsg.Text = "FirstMsg";
            //localBus.Publish(helloMsg);
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }
    }
}
