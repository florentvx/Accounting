using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface IInstitution : IAccountingElement
    {
        string InstitutionName { get; }
        Currency Ccy { get; }
        IEnumerable<IAccount> Accounts { get; }
        IEnumerable<IAccount> GetAccounts(TreeViewMappingElement tvme);
        IAccount TotalAccount(FXMarket mkt, AssetMarket aMkt, Currency convCcy, string overrideAccountName, Price lastAmount);
        IAccount TotalAccount(FXMarket mkt, AssetMarket aMkt, Currency convCcy);
    }
}
