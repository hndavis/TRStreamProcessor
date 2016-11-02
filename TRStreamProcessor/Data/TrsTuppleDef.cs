namespace TRStreamProcessor.Data
{
    public class TrsTuppleDef : TrsTupple
    {
        public TrsTuppleDef(params ITrsTypedField[] fields )
        {
            AddDef(fields);
        }

         public TrsTuppleDef(string name, params ITrsTypedField[] fields)
         {
             Name = name;
            AddDef(fields);
        }

    }
}
