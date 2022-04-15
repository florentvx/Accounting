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
        IEnumerable<Account> Accounts { get; }
        IEnumerable<Account> GetAccounts(TreeViewMappingElement tvme);
    }
}
