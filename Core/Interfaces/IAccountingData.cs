using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAccountingData
    {
        TreeViewMapping Map { get; }
        void ModifyCcy(object valueCcy);
        IEnumerable<ICategory> Categories { get; }
        ICategory GetFirstCategory();
        IAccount Total();
        bool ChangeName(string before, string after, NodeAddress nodeType);
    }
}
