using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Finance
{
    public class CurrencyPair : IMarketInput
    {
        Tuple<Currency, Currency> _Data;

        public CurrencyPair(Currency ccy1, Currency ccy2)
        {
            _Data = new Tuple<Currency, Currency>(ccy1, ccy2);
        }

        public CurrencyPair(ICcyAsset ccy1, ICcyAsset ccy2)
        {
            _Data = new Tuple<Currency, Currency>(ccy1.Ccy, ccy2.Ccy);
        }

        public CurrencyPair(string ccy1, string ccy2)
        {
            _Data = new Tuple<Currency, Currency>(new Currency(ccy1), new Currency(ccy2));
        }

        #region IMarketInput

        public Currency Ccy1 { get { return _Data.Item1; } }

        public Asset Asset1 { get { return null; } }

        public Currency Ccy2 { get { return _Data.Item2; } }

        public object Item1 { get => Ccy1; }

        public ICcyAsset OtherAsset(ICcyAsset ccy)
        {
            if (ccy.Ccy == Ccy1)
                return Ccy2;
            if (ccy.Ccy == Ccy2)
                return Ccy1;
            throw new Exception();
        }

        public bool IsIdentity { get { return Ccy1 == Ccy2; } }

        internal bool IsEqual(CurrencyPair ccyPair)
        {
            return Ccy1 == ccyPair.Ccy1 && Ccy2 == ccyPair.Ccy2;
        }

        public bool IsEqual(IMarketInput imi)
        {
            if (imi.GetType() == typeof(CurrencyPair))
                return IsEqual((CurrencyPair)imi);
            else
                return false;
        }

        internal bool IsEquivalent(CurrencyPair ccyPair)
        {
            if (IsEqual(ccyPair))
                return true;
            else
            {
                CurrencyPair cp = ccyPair.GetInverse();
                return IsEqual(cp);
            }
        }

        public bool IsEquivalent(IMarketInput imi)
        {
            if (imi.GetType() == typeof(CurrencyPair))
                return IsEquivalent((CurrencyPair)imi);
            else
                return false;
        }

        public bool Contains(ICcyAsset ccy)
        {
            return (ccy.Ccy == Ccy1 || ccy.Ccy == Ccy2);
        }

        #endregion

        public override string ToString()
        {
            return $"{Ccy1.ToString()}/{Ccy2.ToString()}";
        }

        public bool Contains(Currency ccy)
        {
            return ccy == Ccy1 || ccy == Ccy2;
        }

        internal CurrencyPair GetInverse()
        {
            return new CurrencyPair(Ccy2, Ccy1);
        }
    }
}
