using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Finance;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Core.Finance
{
    [Serializable]
    public class AssetMarket : Market, IMarket, ISerializable
    {
        [JsonProperty]
        public List<KeyValuePair<AssetCcyPair, double>> _Data;

        public override Dictionary<IMarketInput, double> GetData()
        {
            return _Data.ToDictionary(x => (IMarketInput)x.Key, x => x.Value);
        }

        public override void SetData(Dictionary<IMarketInput, double> input)
        {
            _Data = input.Select(x => new KeyValuePair<AssetCcyPair, double>((AssetCcyPair)x.Key, x.Value)).ToList();
        }
        
        Dictionary<IMarketInput, double> _FXData;

        public AssetMarket()
        {
            _Data = new List<KeyValuePair<AssetCcyPair, double>> { };
            _FXData = new Dictionary<IMarketInput, double> { };
        }

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Dict", _Data, typeof(List<KeyValuePair<AssetCcyPair, double>>));
        }

        public AssetMarket(SerializationInfo info, StreamingContext context)
        {
            _Data = (List<KeyValuePair<AssetCcyPair, double>>)info.GetValue("Dict", typeof(List<KeyValuePair<AssetCcyPair, double>>));
        }

        #endregion

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
            //var dataKeys = _Data.Keys.ToList();
            foreach (KeyValuePair<AssetCcyPair, double> keyValue in _Data)
            {
                AssetCcyPair item = keyValue.Key;
                Asset Asset1 = item.Asset1;
                Currency Ccy2 = item.Ccy2;
                foreach (string ccy in market.GetAvailableCurrencies())
                {
                    Currency fxCcy = new Currency(ccy);
                    CurrencyPair cp = new CurrencyPair(Ccy2, fxCcy);
                    _FXData[new AssetCcyPair(Asset1, fxCcy)] = keyValue.Value * market.GetQuote(cp);
                }
            }
        }
    }
}
