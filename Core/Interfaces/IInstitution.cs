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
        IEnumerable<IAccount> Accounts { get; }
        IEnumerable<IAccount> GetAccounts(TreeViewMappingElement tvme);
    }
}
