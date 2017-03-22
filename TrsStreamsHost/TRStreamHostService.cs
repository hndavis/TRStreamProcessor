using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrsStreamsHost
{
    // https://msdn.microsoft.com/en-us/library/76477d2t(v=vs.110).aspx
    public class TRStreamsHostService : System.ServiceProcess.ServiceBase
    {
        public TRStreamsHostService ()
        {
            this.ServiceName = "TRStreamsHostService";
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }
        public static void Main()
        {
#if DEBUG
            //load modules

            // load comunication plugins
            // load stream modules
#else
            System.ServiceProcess.ServiceBase.Run(new TRStreamsHostService());
#endif
           
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
        }
    }
}
