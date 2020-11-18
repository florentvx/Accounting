using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface IAccountingData
    {
        Currency Ccy { get; }
        TreeViewMapping Map { get; }
        FXMarket FXMarket { get; }
        AssetMarket AssetMarket { get; }
        void ModifyCcy(object valueCcy);
        IEnumerable<ICategory> Categories { get; }
        ICategory GetFirstCategory();
        IAccount Total();
        bool ChangeName(string before, string after, NodeAddress nodeType);
        void AddNewCcy(string ccyName, CurrencyStatics ccyStatics, CurrencyPair ccyPair, double ccyPairQuote);
    }
}
