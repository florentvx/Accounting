using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface ICategory : IAccountingElement
    {
        string CategoryName { get; }
        Currency Ccy { get; }
        IEnumerable<IInstitution> GetInstitutions(TreeViewMappingElement tvm);
        IAccount TotalInstitution(FXMarket mkt, AssetMarket aMkt, Currency convCcy);
        IAccount TotalInstitution(FXMarket mkt, AssetMarket aMkt, Currency convCcy, string overrideName, double? lastAmount);
    }
}
