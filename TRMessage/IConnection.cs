using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMessage
{
    interface IConnection
    {
        String User { get; set; }
        String Password { get; set; }
        String Uri { get; set; }
        void LogOn();

    }
}
