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
        ICcyAsset Ccy { get; }
        IEnumerable<IAccountingElement> GetItemList();
        IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme);
        NodeType GetNodeType();
        IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, Currency ccy, string overrideName);
        void Delete(string v); //Delete an item from the ItemList
        //SummaryReport GetSummary();
        //Price GetTotalAmount(Currency Ccy, FXMarket fxMkt);
    }
}
