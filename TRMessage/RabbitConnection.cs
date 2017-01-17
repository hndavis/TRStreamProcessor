using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMessage
{
    class RabbitConnection : IConnection
    {
        public string Password { get; set; }
        public string Uri { get; set; }
        
        public string User { get; set; }
       
        public void LogOn()
        {
            throw new NotImplementedException();
        }
    }
}
