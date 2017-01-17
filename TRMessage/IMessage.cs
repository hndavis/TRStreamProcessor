using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using TRStreamProcessor.Data;
using TRStreamProcessor.Msg;
using TRStreamProcessor.Stream;

namespace TRMessage
{
    interface IMessage
    {
        String User { get; set; }
        String Password { get; set; }
        String Uri { get; set; }


    }
}
