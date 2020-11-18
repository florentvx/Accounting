using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Finance
{
    public class Asset: IEquatable<Asset>, ICcyAsset
    {
        string _Name;

        public string Name { get { return _Name; } set { _Name = value; } }

        public Asset(string value)
        {
            _Name = value;
        }

        public Asset(object asset)
        {
            _Name = Convert.ToString(asset).ToUpper();
        }

        public Currency Ccy => null;

        Asset ICcyAsset.Asset => this;

        public IMarketInput CreateMarketInput(Currency ccyRef)
        {
            return new AssetCcyPair(this, ccyRef);
        }

        public override string ToString()
        {
            return _Name;
        }

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

        public bool IsCcy()
        {
            return false;
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
    }
}
