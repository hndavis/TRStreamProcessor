using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TRStreamProcessor.Service.Broadcastor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class BroadcastorService : IBroadcastorService
    {

        private static readonly Dictionary<string, IBroadcastorCallBack> Clients = new Dictionary<string, IBroadcastorCallBack>();
        private static readonly object Locker = new object();
        public void NotifyServer(EventDataType eventData)
        {
            lock (Locker)
            {
                var inactiveClients = new List<string>();
                foreach (var client in Clients)
                {
                    if (client.Key != eventData.ClientName)
                    {
                        try
                        {
                            client.Value.BroadcastToClient(eventData);
                        }
                        catch (Exception e)
                        {
                            inactiveClients.Add(client.Key);
                            //todo LOG ex
                        }
                    }
                }

                if (inactiveClients.Count > 0)
                {
                    foreach (var client in inactiveClients)
                    {
                        Clients.Remove(client);
                    }
                }
            }
        }

       
        public void RegisterClient(string clientName) //todo upgrade to guid
        {
            
            if (!string.IsNullOrEmpty(clientName))
            {
                try
                {
                    IBroadcastorCallBack callBack = OperationContext.Current.GetCallbackChannel<IBroadcastorCallBack>();
                    lock (Locker)
                    {
                        // remove old client
                        if (Clients.Keys.Contains(clientName))
                            Clients.Remove(clientName);
                        Clients.Add(clientName, callBack);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}
