using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface IAccountingElement
    {
        string GetName();
        Currency CcyRef { get; }
        IEnumerable<IAccountingElement> GetItemList();
        IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme);
        NodeType GetNodeType();
        IAccount GetTotalAccount(Market mkt, Currency ccyConv);
        IAccount GetTotalAccount(Market mkt, Currency ccyConv, string name);
        void ModifyAmount(Market mkt, string v, object valueAmount);
        void ModifyCcy(Market mkt, string v, object valueCcy, bool isLastRow);
        void Delete(string v); //Delete an item from the ItemList
    }
}
