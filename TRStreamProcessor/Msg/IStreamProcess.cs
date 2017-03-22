using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRStreamProcessor.Stream;

namespace TRStreamProcessor.Msg
{
    interface IStreamProcess
    {
        void Init();
        void SetSource(IStreamProtocolEndPoint spe);
    }
}
