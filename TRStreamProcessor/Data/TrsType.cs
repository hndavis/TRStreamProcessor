using System;
using System.Globalization;
using TRStreamProcessor.Data;

namespace TRStreamProcessor.Data
{
    public enum TrsType
    {
        String,
        Integer,
        Flt,
        Long
    }
    public interface ITrsTypedField
    {
         string Name { get; set; }
         object RawVal { get; }
         TrsType ttype { get; set; }  
    }

    public class TrsTypedField<T> :ITrsTypedField
    {
        public string Name { get; set; }
        public virtual object RawVal
        {
            get { return (object) Val; }
            
        }
        public TrsType ttype { get; set; }
        public T Val { get; set; }
    }

    public class TrsString : TrsTypedField<string>
    {
        public TrsString()
        {
            ttype = TrsType.String;
        }

        public TrsString(string val)
        {
            ttype = TrsType.String;
            Val = val;
        }

       
        public override string ToString()
        {
            return Val;
        }

    }

    public class TrsInt : TrsTypedField<int>
    {
        public TrsInt()
        {
            ttype = TrsType.Integer;
        }
        public TrsInt(int val)
        {
            ttype = TrsType.Integer;
            Val = val;
        }
        

        public override string ToString()
        {
            return Val.ToString();
        }
    }

    public class TrsLong : TrsTypedField<long>
    {
        public TrsLong()
        {
            ttype = TrsType.Long;
        }
        public TrsLong(long val)
        {
            ttype = TrsType.Long;
            Val = val;
        }
        
        public override string ToString()
        {
            return Val.ToString();
        }
    }

    public class TrsFloat : TrsTypedField<double>
    {
        public TrsFloat()
        {
            ttype = TrsType.Flt;
        }

        public TrsFloat(float val)
        {
            ttype = TrsType.Flt;
            Val = val;
        }

        public TrsFloat(double val)
        {
            ttype = TrsType.Flt;
            Val = val;
        }
        

        public override string ToString()
        {
            return Val.ToString(CultureInfo.InvariantCulture);
        }
    }
}