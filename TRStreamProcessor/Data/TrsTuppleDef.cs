namespace TRStreamProcessor.Data
{
    public class TrsTuppleDef : TrsTupple
    {
        public TrsTuppleDef(params TrsTypedField[] fields )
        {
            AddDef(fields);
        }

         public TrsTuppleDef(string name, params TrsTypedField[] fields)
         {
             Name = name;
            AddDef(fields);
        }

    }
}
