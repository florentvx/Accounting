using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface ICcyAsset : ICloneable
    {
        Currency Ccy { get; }
        Asset Asset { get; }
        bool IsCcy();
        IMarketInput CreateMarketInput(Currency ccyRef);
    }
}
