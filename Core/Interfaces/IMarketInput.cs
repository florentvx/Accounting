using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface IMarketInput: IEquatable<IMarketInput>, ISerializable
    {
        Currency Ccy1 { get; }
        Asset Asset1 { get; }
        Currency Ccy2 { get; }
        object Item1 { get; }
        ICcyAsset OtherAsset(ICcyAsset ccy);
        bool IsEqual(IMarketInput other);
        bool IsEquivalent(IMarketInput other);
        bool IsIdentity { get; }
        bool Contains(ICcyAsset ccy);
    }
}
