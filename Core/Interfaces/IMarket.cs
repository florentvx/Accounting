using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Statics;

namespace Core.Interfaces
{
    public interface IMarket
    {
        void AddQuote(IMarketInput mi, double value);
        IEnumerable<Tuple<IMarketInput, double>> EnumerateData();
        IEnumerable<Tuple<IMarketInput, double>> EnumerateData(CurrencyAssetStaticsDataBase ccyDB);
    }
}
