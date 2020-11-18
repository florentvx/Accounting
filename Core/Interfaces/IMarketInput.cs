using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface IMarketInput
    {
        Currency Ccy1 { get; }
        Asset Asset1 { get; }
        Currency Ccy2 { get; }
        object Item1 { get; set; }
        bool IsEqual(IMarketInput other);
        bool IsEquivalent(IMarketInput other);
        bool IsIdentity { get; }
        bool Contains(Currency ccy);
    }
}
