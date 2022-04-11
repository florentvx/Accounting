using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Newtonsoft.Json;

namespace Core.Finance
{
    [Serializable]
    public class AssetCcyPair : IMarketInput
    {
        [JsonProperty]
        public Asset Asset { get; set; }

        [JsonProperty]
        public Currency CcyPrice { get; set; }

        public AssetCcyPair(Asset asset, Currency ccy)
        {
            Asset = asset;
            CcyPrice = ccy;
        }

        #region IMarketInput

        public bool IsIdentity => false;

        public Currency Ccy => null;

        public ICcyAsset Other(ICcyAsset ccy)
        {
            if (ccy.Ccy == CcyPrice) return Asset;
            if (ccy.Asset == Asset) return CcyPrice;
            return null;
        }

        public bool IsEqual(IMarketInput other)
        {
            return other.Asset == Asset && other.CcyPrice == CcyPrice;
        }

        public bool IsEquivalent(IMarketInput other)
        {
            return IsEqual(other);
        }
        
        public bool Contains(ICcyAsset ccy)
        {
            return ccy.Ccy == CcyPrice || ccy.Asset == Asset;
        }

        public object Clone()
        {
            return new AssetCcyPair((Asset)Asset.Clone(), (Currency)CcyPrice.Clone());
        }

        #endregion

        #region IEquatable

        public bool Equals(IMarketInput imi)
        {
            if (imi == null)
                return false;
            return Asset == imi.Asset && CcyPrice == imi.CcyPrice;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IMarketInput);
        }

        public override int GetHashCode()
        {
            return Asset.GetHashCode() + CcyPrice.GetHashCode();
        }

        public static bool operator ==(AssetCcyPair imi1, IMarketInput imi2)
        {
            if (imi1 is null)
            {
                if (imi2 is null) { return true; }
                return false;
            }
            return imi1.Equals(imi2);
        }

        public static bool operator !=(AssetCcyPair imi1, IMarketInput imi2)
        {
            return !(imi1 == imi2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Asset1", Asset, typeof(Asset));
            info.AddValue("Ccy2", CcyPrice, typeof(Currency));
        }

        public AssetCcyPair(SerializationInfo info, StreamingContext context)
        {
            Asset = (Asset)info.GetValue("Asset1", typeof(Asset));
            CcyPrice = (Currency)info.GetValue("Ccy2", typeof(Currency));
        }

        #endregion


    }
}
