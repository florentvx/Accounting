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

        public Currency Ccy1 => null;

        public Asset Asset1 => Asset;

        public Currency Ccy2 => Ccy;

        object IMarketInput.Item1 { get => Asset1; set => throw new NotImplementedException(); }

        public bool IsIdentity => false;

        public bool Contains(Currency ccy)
        {
            return Ccy.Equals(ccy);
        }

        public bool IsEqual(IMarketInput other)
        {
            return other.Asset1 == Asset1 && other.Ccy2 == Ccy;
        }

        public bool IsEquivalent(IMarketInput other)
        {
            return IsEqual(other);
        }

    }
}
