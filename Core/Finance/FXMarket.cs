using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Finance
{
    public class FXMarket : Market, IMarket
    {
        //CurrencyStaticsDataBase _CcyDB;
        Currency CcyRef;

        public FXMarket(Currency ccy) {
            CcyRef = ccy;
        }

        //public void SetCcyStaticsDB(CurrencyStaticsDataBase ccyDB)
        //{
        //    _CcyDB = ccyDB;
        //}

        //public bool AddCcy(string newCcy, CurrencyStatics statics)
        //{
        //    return _CcyDB.AddCcy(newCcy, statics);
        //}

        //public bool AddCcy(string newCcy, string symbol, int thousandMark, int decNb)
        //{
        //    return _CcyDB.AddCcy(newCcy, new CurrencyStatics(symbol, thousandMark, decNb));
        //}

        //public override void Reset()
        //{
        //    base.Reset();
        //    //_CcyDB.Reset();
        //}

        private IEnumerable<ICcyAsset> GetAllConnectedCcy(ICcyAsset ccy, List<ICcyAsset> excludedCcies)
        {
            return _Data.Where(x => x.Key.Contains(ccy) && !excludedCcies.Contains(x.Key.OtherAsset(ccy)))
                        .Select(x => x.Key.OtherAsset(ccy));
        }

        private double Aux_GetQuote(ICcyAsset ccy, ICcyAsset ccy2, List<ICcyAsset> excludedCcies)
        {
            var connectedCcies = GetAllConnectedCcy(ccy, excludedCcies);
            if (connectedCcies.Count() == 0)
                return 0;
            else if (connectedCcies.Contains(ccy2))
                return GetQuote(new CurrencyPair(ccy, ccy2));
            else
            {
                foreach (var ccy_k in connectedCcies)
                {
                    List<ICcyAsset> exL_k = excludedCcies.Select(x => (ICcyAsset)x.Ccy.Clone()).ToList();
                    exL_k.Add(ccy);
                    double value = GetQuote(new CurrencyPair(ccy, ccy_k));
                    value *= Aux_GetQuote(ccy_k.Ccy, ccy2, exL_k);
                    if (value != 0)
                        return value;
                }
            }
            throw new Exception("Error in Aux_GetQuote()");
        }

        public double GetQuote(IMarketInput ccyPair)
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
                return Aux_GetQuote(ccyPair.Ccy1, ccyPair.Ccy2, new List<ICcyAsset> { });
            }
            catch (Exception)
            {
                throw new Exception($"Issue with Data for {ccyPair.ToString()}");
            }
        }

        //public string CcyToString(Currency ccy, double value)
        //{
        //    return _CcyDB.CcyToString(ccy, value);
        //}

        public IEnumerable<string> GetAvailableCurrencies()
        {
            var Ccy1List = _Data.Select(x => x.Key.Ccy1.ToString());
            var Ccy2List = _Data.Select(x => x.Key.Ccy2.ToString());
            var res = Ccy1List.Union(Ccy2List).ToList();
            if (!res.Contains(CcyRef.ToString()))
                res.Add(CcyRef.ToString());
            return res;
        }

        internal void SetCcyRef(Currency ccy)
        {
            CcyRef = ccy;
        }

        public bool IsCcy(string ccy)
        {
            return GetAvailableCurrencies().Contains(ccy);
        }
    }
}
