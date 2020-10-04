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
        IAccount TotalAccount();
        IAccount TotalAccount(string overrideAccountName);
        void Modify(string accountName, string columnName, object value);
    }
}
