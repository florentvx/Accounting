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
        void AddQuote(IMarketInput mi, double value);
        double GetQuote(IMarketInput mi);
        IEnumerable<Tuple<IMarketInput, double>> EnumerateData();
    }
}
