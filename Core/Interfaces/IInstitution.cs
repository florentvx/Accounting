using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IInstitution
    {
        string InstitutionName { get; }
        Currency Ccy { get; }
        IEnumerable<IAccount> Accounts { get; }
        IEnumerable<IAccount> GetAccounts(TreeViewMappingElement tvme);
        IAccount TotalAccount();
        IAccount TotalAccount(string overrideAccountName);
        void ModifyAmount(string accountName, object value);
        void ModifyCcy(string accountName, object value, bool IsLastRow);
        void ChangeName(string before, string after, NodeAddress nodeType);
    }
}
