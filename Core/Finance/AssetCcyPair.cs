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
        public Currency Ccy { get; set; }

        public AssetCcyPair(Asset asset, Currency ccy)
        {
            Asset = asset;
            Ccy = ccy;
        }

        #region IMarketInput

        public Currency Ccy1 => null;

        public Asset Asset1 => Asset;

        public Currency Ccy2 => Ccy;

        public object Item1 { get => Asset1; }

        public ICcyAsset OtherAsset(ICcyAsset ccy)
        {
            if (ccy.Ccy == Ccy)
                return Asset;
            if (ccy.Asset == Asset)
                return Ccy;
            throw new Exception();
        }

        public bool IsIdentity => false;

        public bool IsEqual(IMarketInput other)
        {
            return other.Asset1 == Asset1 && other.Ccy2 == Ccy;
        }

        public bool IsEquivalent(IMarketInput other)
        {
            return IsEqual(other);
        }
        
        public bool Contains(ICcyAsset ccy)
        {
            return ccy.Ccy == Ccy || ccy.Asset == Asset;
        }

        #endregion

        #region IEquatable

        public bool Equals(IMarketInput imi)
        {
            if (imi == null)
                return false;
            return Asset1 == imi.Asset1 && Ccy2 == imi.Ccy2;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IMarketInput);
        }

        public override int GetHashCode()
        {
            return Asset1.GetHashCode() + Ccy2.GetHashCode();
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
            info.AddValue("Ccy2", Ccy, typeof(Currency));
        }

        public AssetCcyPair(SerializationInfo info, StreamingContext context)
        {
            Asset = (Asset)info.GetValue("Asset1", typeof(Asset));
            Ccy = (Currency)info.GetValue("Ccy2", typeof(Currency));
        }

        #endregion


    }
}
