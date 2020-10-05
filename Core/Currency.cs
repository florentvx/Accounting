using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public enum Currency
    {
        USD,EUR,GBP,JPY,None
    }

    public static class CurrencyFunctions
    {
        public static bool IsNone(this Currency ccy)
        {
            return ccy == Currency.None;
        }

        public static List<Currency> GetCurrencyList(bool withoutNone = true)
        {
            List<Currency> res = new List<Currency> { };
            foreach (Currency ccy in Enum.GetValues(typeof(Currency)))
                if (!(withoutNone && ccy.IsNone()))
                    res.Add(ccy);
            return res;
        }

        internal static Currency ToCurrency(object value)
        {
            return (Currency)Enum.Parse(typeof(Currency), Convert.ToString(value));
        }
    }
}
