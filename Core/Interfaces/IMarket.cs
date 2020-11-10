using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface IMarket
    {
        void AddQuote(CurrencyPair cp, double value);
    }
}
