using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRStreamProcessor.Data
{
    public class TrsTuppleFactory
    {
        private readonly TrsTuppleDef Definition;
        public TrsTuppleFactory(TrsTuppleDef definition)
        {
            Definition = definition;
        }

        public TrsTupple Create(params ITrsTypedField[] fields)
        {
            if ( Definition.orderedFieldNames.Count != fields.Length )
                throw new Exception("Wrong number of Fields");
         
            for (var i=0; i< Definition.orderedFieldNames.Count; i++ )
            {
                if (Definition.Fields[Definition.orderedFieldNames[i]].ttype !=
                    fields[i].ttype)
                    throw new Exception("Fields types incompatible " + fields[i].Name + " :" +i);
                
            }
                 var t = new TrsTupple(Definition, fields);

            return t;
        }

    }
    
}
