using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Statics
{
    [Serializable]
    public class CurrencyStatics: IEquatable<CurrencyStatics>, ISerializable
    {
        private string _Name;
        private string _Symbol;
        private int _ThousandMark;
        private int _DecimalNumber;
        private CurrencyPair _PricingCcyPair;

        public string Name { get { return _Name; } }
        public string Symbol { get { return _Symbol; } set { _Symbol = value; } }
        public int ThousandMark { get { return _ThousandMark; } set { _ThousandMark = value; } }
        public int DecimalNumber { get { return _DecimalNumber; } set { _DecimalNumber = value; } }
        public CurrencyPair PricingCcyPair { get { return _PricingCcyPair; } }

        public CurrencyStatics() { }

        public CurrencyStatics(string name, string symbol, int thousandMark, int decNb, CurrencyPair cp = null)
        {
            _Name = name;
            _Symbol = symbol;
            _ThousandMark = thousandMark;
            _DecimalNumber = decNb;
            _PricingCcyPair = cp;
        }

        private int GetDigit(double value, int pos, int N)
        {
            return Convert.ToInt32(Math.Floor(value * Math.Pow(10, -(N - 1 - pos))));
        }

        private void GetDigitAndUpdateValue(ref string res, ref double value, int pos, int N)
        {
            int digit = GetDigit(value, pos, N);
            res += Convert.ToString(digit);
            value -= digit * Math.Pow(10, N - 1 - pos);
        }

        public string ValueToString(double value)
        {
            string res = "";
            if (value < 0)
            {
                res = "-"; 
                value = -value;
            }
            value = Math.Round(value, _DecimalNumber);
            double roundedvalue = Math.Floor(value);
            if (_DecimalNumber == 0)
                roundedvalue = Math.Round(value, 0);
            if (roundedvalue == 0)
                res += "0";
            else
            {
                int N = Convert.ToInt32(Math.Floor(Math.Log10(roundedvalue))) + 1;
                int introNb = 0;
                if (N > _ThousandMark)
                {
                    introNb = N % _ThousandMark;
                    for (int i = 0; i < introNb; i++)
                    {
                        GetDigitAndUpdateValue(ref res, ref roundedvalue, i, N);
                    }
                    if (introNb > 0)
                        res += ",";
                }
                int pos = 0;
                for (int j = introNb; j < N; j++)
                {
                    GetDigitAndUpdateValue(ref res, ref roundedvalue, j, N);
                    pos++;
                    if (pos % _ThousandMark == 0 && j < N - 1)
                        res += ",";
                }
            }
            // Decimal Part
            if (_DecimalNumber > 0)
            {
                double decimalPart = Convert.ToInt32(Math.Round((value - Math.Floor(value)) * Math.Pow(10, _DecimalNumber)));
                if (decimalPart > 0)
                {
                    int N_dec = Convert.ToInt32(Math.Floor(Math.Log10(decimalPart))) + 1;
                    res += ".";
                    for (int k_0 = N_dec; k_0 < _DecimalNumber; k_0++)
                    {
                        res += "0";
                    }
                    for (int k = 0; k < N_dec; k++)
                    {
                        GetDigitAndUpdateValue(ref res, ref decimalPart, k, N_dec);
                    }
                }
            }
            return _Symbol + " " + res;
        }

        public Tuple<bool, string> Load(string name, string symbol, string thousandMarker, string decimalNumber, CurrencyPair cp)
        {
            // Name
            if (name.Length > 2 && name.Length < 5)
            {
                _Name = name;
            }
            else
                return new Tuple<bool, string>(false, $"Name [{name}] should have between 3 and 4 characters.");

            // Symbol
            if (symbol.Length < 6)
            {
                _Symbol = symbol;
            }
            else
                return new Tuple<bool, string>(false, $"Symbol [{symbol}] is more than 5 characters.");
            
            // Thousand Marker
            try
            {
                int tMkNb = Convert.ToInt32(thousandMarker);
                _ThousandMark = tMkNb;
            }
            catch(Exception)
            {
                throw new Exception($"The thousand Marker [{thousandMarker}] needs to be a number.");
            }

            // Decimal Number
            try
            {
                int dNb = Convert.ToInt32(decimalNumber);
                _DecimalNumber = dNb;
            }
            catch (Exception)
            {
                throw new Exception($"The thousand Marker [{_DecimalNumber}] needs to be a number.");
            }

            // Pricing CcyPair
            _PricingCcyPair = cp;
            return new Tuple<bool, string>(true, null);
        }


        #region IEquatable

        public bool Equals(CurrencyStatics cs)
        {
            if (cs == null)
                return false;
            return _Symbol == cs._Symbol 
                && _DecimalNumber == cs._DecimalNumber
                && _ThousandMark == cs._ThousandMark;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as CurrencyStatics);
        }

        public override int GetHashCode()
        {
            return _Symbol.GetHashCode() + _DecimalNumber.GetHashCode() + _ThousandMark.GetHashCode();
        }

        public static bool operator ==(CurrencyStatics cs1, CurrencyStatics cs2)
        {
            if (cs1 is null)
            {
                if (cs1 is null) { return true; }
                return false;
            }
            return cs1.Equals(cs2);
        }

        public static bool operator !=(CurrencyStatics cs1, CurrencyStatics cs2)
        {
            return !(cs1 == cs2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", _Name, typeof(string));
            info.AddValue("Symbol", _Symbol, typeof(string));
            info.AddValue("DecimalNumber", _DecimalNumber, typeof(string));
            info.AddValue("ThousandMark", _ThousandMark, typeof(string));
            info.AddValue("PricingCcyPair", _PricingCcyPair, typeof(CurrencyPair));
        }

        public CurrencyStatics(SerializationInfo info, StreamingContext context)
        {
            _Name = (string)info.GetValue("Name", typeof(string));
            _Symbol = (string)info.GetValue("Symbol", typeof(string));
            _DecimalNumber = (int)info.GetValue("DecimalNumber", typeof(int));
            _ThousandMark = (int)info.GetValue("ThousandMark", typeof(int));
            _PricingCcyPair = (CurrencyPair)info.GetValue("PricingCcyPair", typeof(CurrencyPair));
        }

        #endregion
    }
}
