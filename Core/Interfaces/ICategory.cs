using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface ICategory
    {
        string CategoryName { get; }
        Currency Ccy { get; }
        IEnumerable<IInstitution> GetInstitutions(TreeViewMappingElement tvm);
        IAccount TotalInstitution(Market mkt, Currency convCcy);
        IAccount TotalInstitution(Market mkt, Currency convCcy, string overrideName);
    }
}
