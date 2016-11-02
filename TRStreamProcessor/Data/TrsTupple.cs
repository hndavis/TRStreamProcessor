using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace TRStreamProcessor.Data
{
    public class TrsTupple : iTupple
    {
        public string Name { get; set; }
        public Dictionary<String, ITrsTypedField> Fields = new Dictionary<string, ITrsTypedField>();
        public DateTime CreateDate = DateTime.Now;
        public readonly List<String> orderedFieldNames = new List<string>();
        Guid id = Guid.NewGuid();
        public  TrsTuppleDef Definition;

        public TrsTupple()
        {
        }

        public TrsTupple(TrsTuppleDef tuppleDef)
        {
            Definition = tuppleDef;

        }

        public TrsTupple(TrsTuppleDef tuppleDef, ITrsTypedField[] fields)
        {
            Definition = tuppleDef;
            Add(fields);
        }


        public TrsTupple(string name)
        {
            Name = name;
        }

        public TrsTupple(string name, params ITrsTypedField[] fields)
        {
            Name = name;
            Add(fields);

        }

        public TrsTupple(params ITrsTypedField[] fields)
        {
                Add(fields);
        }

        protected bool Add(ITrsTypedField[] fields)
        {
            foreach (var field in fields)
            {
                Add(field);
            }
            return true;
        }



        protected bool AddDef(ITrsTypedField fld)
        {
           // if ( orderedFieldNames.Count == 0)
            orderedFieldNames.Add(fld.Name);
            Fields[fld.Name] = fld;
            return true;
        }

        protected bool AddDef(ITrsTypedField[] fields)
        {
            foreach (var field in fields)
            {
                AddDef(field);
            }
            return true;
        }



        protected bool Add(ITrsTypedField fld)
        {
            // if ( orderedFieldNames.Count == 0)
            if ( string.IsNullOrEmpty(fld.Name ))
                throw new Exception("Field Name cannot be empty");
           
            Fields[fld.Name] = fld;
            return true;
        }

        public new string ToString()
        {
            StringBuilder bldr = new StringBuilder();
            foreach (var name in Definition.orderedFieldNames)
            {
                
                bldr.Append(Fields[name]);
                bldr.Append(" :: ");
            }
            return bldr.ToString();
        }
        String Id { get { return id.ToString(); } }

        public string FieldNames()
        {
            StringBuilder bldr = new StringBuilder();
            foreach (var name in orderedFieldNames)
            {
                bldr.Append(name + "    ");
            }
            return bldr.ToString();
        }

        public ITrsTypedField GetColumn(string name)
        {

            return Fields[name];
        }
    }
}
