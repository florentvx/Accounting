using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface ICcyAsset
    {
        Currency Ccy { get; }
        Asset Asset { get; }
        IMarketInput CreateMarketInput(Currency ccyRef);
        bool IsCcy();
    }
}
