using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFBroadCastClient
{
    class Program
    {
        Guid clientId = Guid.NewGuid();
        private BroadcastorService.BroadcastorServiceClient _client;
        static void Main(string[] args)
        {
            var p = new Program();
            Console.WriteLine(" Registering Client = {0} ", p.clientId.ToString());
          
            p.RegisterClient();
            Console.WriteLine(" Registered Client = {0} ", p.clientId.ToString());
            Console.ReadLine();
            p.SendEvent("Hi");
            Console.ReadLine();

        }

        private delegate void HandleBroadcastCallback(object sender, EventArgs e);
        public void HandleBroadcast(object sender, EventArgs e)
        {
            try
            {
                var eventData = (BroadcastorService.EventDataType)sender;
                Console.WriteLine("{0} ( from {1}", eventData.EventMessage, eventData.ClientName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private void RegisterClient()
        {
            if (this._client != null)
            {
                this._client.Abort();
                this._client = null;
            }

            BroadcastorCallback cb = new BroadcastorCallback();
            cb.SetHandler(this.HandleBroadcast);

            System.ServiceModel.InstanceContext context =
                    new System.ServiceModel.InstanceContext(cb);
            this._client =
                new BroadcastorService.BroadcastorServiceClient(context);

            this._client.RegisterClient(clientId.ToString());
        }

        private void SendEvent(string msg)
        {
            if ( this._client==null)
            {
                Console.WriteLine("Client is not registered.");
            }
            this._client.NotifyServer(
                new BroadcastorService.EventDataType()
                {
                    ClientName = this.clientId.ToString(),
                    EventMessage = msg
                });
        }

    }
}
