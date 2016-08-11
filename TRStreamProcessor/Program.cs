using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRStreamProcessor.Data;
using TRStreamProcessor.Stream;

namespace TRStreamProcessor
{
    class Program
    {
        static void Main(string[] args)
        {

            var entryPoint = new TrsStream<TrsTupple, TrsTupple>();


            string col1 = "TestAppId";
            string col2 = "TestUserId";
            string col3 = "NumDataPoints";
            var applicationIdDef = new TrsString("TestAppId") { Name = col1 };
            var userNameDef = new TrsString("TestUserId") { Name = col2 };
            var dataPointsDef = new TrsInt(0) { Name = col3 };

            //var listenTask = Task.Factory.StartNew(() =>
            //        {
            //            var listener = new TrsListener(entryPoint.GetOutStream());
            //           // listener.Listen();

            //        }
            //    );
            var groupbyColumns = new List<TrsTypedField> {applicationIdDef, userNameDef};
            var computeColumns = new List<TrsTypedField> {dataPointsDef};
            var winTask = Task.Factory.StartNew(() =>
            {
                var simpleWindowSum = new TrsWindow<TrsTupple, TrsTupple>(entryPoint.GetOutStream(),
                    groupbyColumns, computeColumns, 60, TrsWindowType.MaxNum);
                simpleWindowSum.Listen();
            });

            winTask.Wait(); // make sure subscribed

            var defDataPointTupple = new TrsTuppleDef(applicationIdDef, userNameDef, dataPointsDef);
            TrsTuppleFactory applDataPointFactory = new TrsTuppleFactory(defDataPointTupple);

            var rd = new Random();


            //listenTask.Wait();
            //create rows

            var taskAddData1 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    var applicationId = new TrsString("TestAppId") {Name = col1};
                    var userName = new TrsString("TestUserIdOne") {Name = col2};
                    // var dataPoints = new TrsInt(rd.Next()) { Name = col3 };
                    var dataPoints = new TrsInt(2) {Name = col3};

                    //var sampleTuple = new TrsTupple("Sample"+i.ToString(),
                    //   (TrsTypedField)applicationId, userName, dataPoints);

                    var sampleTuple = applDataPointFactory.Create(applicationId, userName, dataPoints);

                    entryPoint.pub(sampleTuple);
                    Thread.Sleep(5000);

                }
            });

            var taskAddData2 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    var applicationId = new TrsString("TestAppId") { Name = col1 };
                    var userName = new TrsString("TestUserIdTwo") { Name = col2 };
                    // var dataPoints = new TrsInt(rd.Next()) { Name = col3 };
                    var dataPoints = new TrsInt(3) { Name = col3 };

                    //var sampleTuple = new TrsTupple("Sample"+i.ToString(),
                    //   (TrsTypedField)applicationId, userName, dataPoints);

                    var sampleTuple = applDataPointFactory.Create(applicationId, userName, dataPoints);

                    entryPoint.pub(sampleTuple);
                    Thread.Sleep(4000);

                }
            });


            var taskAddData3 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    var applicationId = new TrsString("TestAppIdTwo") { Name = col1 };
                    var userName = new TrsString("TestUserIdTwo") { Name = col2 };
                    // var dataPoints = new TrsInt(rd.Next()) { Name = col3 };
                    var dataPoints = new TrsInt(3) { Name = col3 };

                    //var sampleTuple = new TrsTupple("Sample"+i.ToString(),
                    //   (TrsTypedField)applicationId, userName, dataPoints);

                    var sampleTuple = applDataPointFactory.Create(applicationId, userName, dataPoints);

                    entryPoint.pub(sampleTuple);
                    Thread.Sleep(4500);

                }
            });
            Thread.Sleep(200000);
            winTask.Dispose();
        }
    }
}
