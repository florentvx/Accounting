using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface IAccountingElement : ICloneable
    {
        string GetName();
        ICcyAsset Ccy { get; }
        IAccountingElement GetItem(NodeAddress na);
        TreeViewMapping GetTreeStructure();
        NodeType GetNodeType();
        IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, Currency ccy, string overrideName);
        IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, Currency ccy);
        Price GetTotalAmount(FXMarket mkt, AssetMarket aMkt, Currency ccy);
        void Delete(string name); //Delete an item from the ItemList
        SummaryReport GetSummary();
        
    }
}
