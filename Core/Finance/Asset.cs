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
    public class Asset: ICcyAsset, IEquatable<Asset>, ISerializable
    {
        string _Name;

        [JsonProperty]
        public string Name { get { return _Name; } set { _Name = value; } }

        public Asset(string value)
        {
            _Name = value;
        }

        public Asset(object asset)
        {
            _Name = Convert.ToString(asset).ToUpper();
        }

        public override string ToString()
        {
            return _Name;
        }

        #region ICcyAsset

        public Currency Ccy => null;

        Asset ICcyAsset.Asset => this;

        public bool IsCcy()
        {
            return false;
        }

        public IMarketInput CreateMarketInput(Currency ccyRef)
        {
            return new AssetCcyPair(this, ccyRef);
        }

        #endregion

        #region IEquatable

        public bool Equals(Asset other)
        {
            return _Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Asset);
        }

        public override int GetHashCode()
        {
            return _Name.GetHashCode();
        }

        public static bool operator ==(Asset asset1, Asset asset2)
        {
            if (asset1 is null)
            {
                if (asset2 is null) { return true; }
                return false;
            }
            return asset1.Equals(asset2);
        }

        public static bool operator !=(Asset asset1, Asset asset2)
        {
            return !(asset1 == asset2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Asset", _Name, typeof(string));
        }

        public Asset(SerializationInfo info, StreamingContext context)
        {
            _Name = (string)info.GetValue("Asset", typeof(string));
        }

        #endregion
    }
}
