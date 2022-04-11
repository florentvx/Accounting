using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface IMarketInput: IEquatable<IMarketInput>, ISerializable, ICloneable
    {
        Currency Ccy { get; }
        Asset Asset { get; }
        Currency CcyPrice { get; }

        ICcyAsset Other(ICcyAsset ccy);

        bool IsIdentity { get; }
        bool IsEqual(IMarketInput other);
        bool IsEquivalent(IMarketInput other);
        bool Contains(ICcyAsset ccy);
    }
}
