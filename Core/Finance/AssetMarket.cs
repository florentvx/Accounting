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

        public AssetMarket()
        {
            _Data = new List<KeyValuePair<AssetCcyPair, double>> { };
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

        public IEnumerable<Asset> GetAvailableAssets()
        {
            return EnumerateData().Select(x => x.Item1.Asset);
        }

        internal bool ContainsAsset(Asset asset)
        {
            return GetAvailableAssets().Contains(asset);
        }

        public Price PriceAsset(Price price)
        {
            if (price.Ccy.IsCcy())
                throw new InvalidOperationException($"price has to be an asset quantity: {price}");
            KeyValuePair<AssetCcyPair, double> data = _Data .Where(x => x.Key.Asset == price.Ccy.Asset)
                                                            .Select(x => x)
                                                            .First();
            return new Price(price.Amount * data.Value, data.Key.CcyPrice);
        }

        
    }
}
