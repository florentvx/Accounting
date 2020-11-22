using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Finance
{
    public class AssetCcyPair : IMarketInput
    {
        public Asset Asset;
        public Currency Ccy;

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

    }
}
