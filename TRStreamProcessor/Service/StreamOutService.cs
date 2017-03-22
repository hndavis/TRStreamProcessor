using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRStreamProcessor.Service
{
    public class StreamOutService : IStreamOutService
    {
        public string GetMessageQueueName()
        {
            return "BasicQ";
        }
    }
}
