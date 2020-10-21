using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICategory
    {
        string CategoryName { get; }
        IEnumerable<IInstitution> GetInstitutions(TreeViewMappingElement tvm);
        IAccount TotalInstitution();
        IAccount TotalInstitution(string overrideName);
    }
}
