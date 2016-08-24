using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ.Events;

namespace TRStreamProcessor.Msg
{
    public class Hello
    {
        public string Text { get; set; }
    }
}
