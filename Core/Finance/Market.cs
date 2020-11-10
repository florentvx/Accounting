using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Finance
{
    public class Market : IMarket
    {
        Dictionary<CurrencyPair, double> _Data = new Dictionary<CurrencyPair, double> { };
        List<CurrencyPair> _CcyPairs = new List<CurrencyPair> { };

        public Market() { }

        public IEnumerable<Tuple<CurrencyPair, double>> EnumerateData()
        {
            return _CcyPairs.Select(x => new Tuple<CurrencyPair, double>(x, _Data[x]));
        }

        public void Reset()
        {
            _Data = new Dictionary<CurrencyPair, double> { };
            _CcyPairs = new List<CurrencyPair> { };
        }

        public double GetQuote(CurrencyPair ccyPair)
        {
            if (ccyPair.IsIdentity)
                return 1.0;
            
            // Find exact CcyPair
            var presentData = _Data .Where(x => x.Key.IsEquivalent(ccyPair))
                                    .Select(x => x.Key)
                                    .ToList();
            if (presentData.Count() == 1)
            {
                if (presentData[0].IsEqual(ccyPair))
                    return _Data[presentData[0]];
                else
                    return 1 / _Data[presentData[0]];
            }

            // Find triangle
            var ccyPairData = _Data.Where(x => x.Key.Contains(ccyPair.Ccy1) || x.Key.Contains(ccyPair.Ccy2));
            var ccyData = ccyPairData   .Select(x => x.Key.Ccy1 == ccyPair.Ccy1 || x.Key.Ccy1 == ccyPair.Ccy2 ? x.Key.Ccy2 : x.Key.Ccy1)
                                        .ToList();
            
            try
            {
                Currency ccy3 = ccyData.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.First()).First();
                double ccy1ccy3 = GetQuote(new CurrencyPair(ccyPair.Ccy1, ccy3));
                double ccy3ccy2 = GetQuote(new CurrencyPair(ccy3, ccyPair.Ccy2));
                return ccy1ccy3 * ccy3ccy2;
            }
            catch (Exception)
            {
                throw new Exception($"Issue with Data for {ccyPair.ToString()}");
            }
        }

        public void AddQuote(CurrencyPair ccyPair, double value)
        {
            var presentData = _Data.Where(x => x.Key.IsEquivalent(ccyPair)).Select(x=> x.Key).ToList();
            if (presentData.Count() == 1)
            {
                if (presentData[0].IsEqual(ccyPair))
                    _Data[presentData[0]] = value;
                else
                    _Data[presentData[0]] = 1 / value;
            }
            else if (presentData.Count() == 0)
            {
                _Data[ccyPair] = value;
                _CcyPairs.Add(ccyPair);
            }
        }
    }
}
