using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TRStreamProcessor.Service.Broadcastor
{
    [DataContract()]

   
        public class EventDataType
        {
            [DataMember]
            public string ClientName { get; set; }

            [DataMember]
            public string EventMessage { get; set; }
        }

   
}
