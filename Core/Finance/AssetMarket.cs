using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Finance;

namespace Core.Finance
{
    public class AssetMarket : Market, IMarket
    {
        Dictionary<IMarketInput, double> _FXData;

        public AssetMarket()
        {
            _FXData = new Dictionary<IMarketInput, double> { };
        }

        public IEnumerable<string> GetAvailableAssets()
        {
            return EnumerateData().Select(x => x.Item1.Asset1.ToString());
        }

        internal bool ContainsAsset(string assetName)
        {
            return GetAvailableAssets().Contains(assetName);
        }

        public double GetQuote(IMarketInput mi)
        {
            var miList = _FXData  .Where(x => x.Key.Asset1 == mi.Asset1 && x.Key.Ccy2 == mi.Ccy2)
                                .Select(x => x.Key);
            if (miList.Count() == 1)
                return _FXData[miList.First()];
            else
                throw new Exception("ERROR: Asset Market");
        }

        public void PopulateWithFXMarket(FXMarket market)
        {
            _FXData = new Dictionary<IMarketInput, double> { };
            var dataKeys = _Data.Keys.ToList();
            foreach (IMarketInput item in dataKeys)
            {
                Asset Asset1 = item.Asset1;
                Currency Ccy2 = item.Ccy2;
                foreach (string ccy in market.GetAvailableCurrencies())
                {
                    Currency fxCcy = new Currency(ccy);
                    CurrencyPair cp = new CurrencyPair(Ccy2, fxCcy);
                    _FXData[new AssetCcyPair(Asset1, fxCcy)] = _Data[item] * market.GetQuote(cp);
                }
            }
        }
    }
}
