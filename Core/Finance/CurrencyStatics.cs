using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Finance
{
    public class CurrencyStatics
    {
        private string _Symbol;
        private int _ThousandMark;
        private int _DecimalNumber;

        public CurrencyStatics() { }

        public CurrencyStatics(string symbol, int thousandMark, int decNb)
        {
            _Symbol = symbol;
            _ThousandMark = thousandMark;
            _DecimalNumber = decNb;
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

        internal string ValueToString(double value)
        {
            string res = "";
            double roundedvalue = Math.Floor(value);
            if (_DecimalNumber == 0)
                roundedvalue = Math.Round(value, 0);
            if (roundedvalue == 0)
                res = "0";
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
                    for (int k = 0; k < N_dec; k++)
                    {
                        GetDigitAndUpdateValue(ref res, ref decimalPart, k, N_dec);
                    }
                }
            }
            return _Symbol + " " + res;
        }

        public Tuple<bool, string> Load(string symbol, string thousandMarker, string decimalNumber)
        {
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
            return new Tuple<bool, string>(true, null);
        }
    }

    public class CurrencyStaticsDataBase
    {
        Dictionary<string, CurrencyStatics> DataBase = new Dictionary<string, CurrencyStatics> { };

        public CurrencyStaticsDataBase() {}

        public void Reset()
        {
            DataBase = new Dictionary<string, CurrencyStatics> { };
        }

        public bool AddCcy(string newCcy, CurrencyStatics cs)
        {
            if (DataBase.ContainsKey(newCcy))
                return false;
            else
            {
                DataBase.Add(newCcy, cs);
                return true;
            }
        }

        public IEnumerable<string> GetAvailableCurrencies()
        {
            return DataBase.Keys;
        }

        internal string CcyToString(Currency ccy, double value)
        {
            CurrencyStatics cs = DataBase[ccy.ToString()];
            return cs.ValueToString(value);
        }
    }
}
