using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAccountingData
    {
        Category GetCategory(string catName);
        IEnumerable<ICategory> Categories { get; }
        IInstitution GetInstitution(string catName, string instName);
        IAccount Total();
        Dictionary<string, Dictionary<string, List<string>>> GetSummary();
        ICategory GetFirstCategory();
        void ChangeName(string before, string after, NodeAddress nodeType);
        void ModifyCcy(object valueCcy);
        TreeViewMapping Map { get; }
    }
}
