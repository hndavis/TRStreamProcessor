using System;
using System.Globalization;
using TRStreamProcessor.Data;

namespace TRStreamProcessor.Data
{
    public enum TrsType
    {
        String,
        Integer,
        Flt
    }
    public class TrsTypedField
    {
        public string Name { get; set; }
        public  object RawVal;
        public TrsType ttype { get; set; }  
    }

    public class TrsString : TrsTypedField
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

        public  String Val {
            get { return (string)RawVal; }
            set { RawVal = value; }
        }

        public override string ToString()
        {
            return Val;
        }

    }

    public class TrsInt : TrsTypedField
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
        public int Val {
            get { return (int) RawVal; } 
            set { RawVal = value; } }

        public override string ToString()
        {
            return Val.ToString();
        }
    }

    public class TrsFloat : TrsTypedField
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



        public double Val {
            get
            {
               
                 return (double) RawVal;
             
            }
            set { RawVal = value; }
        }

        public override string ToString()
        {
            return Val.ToString(CultureInfo.InvariantCulture);
        }
    }
}