﻿using System;
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
        ICcyAsset CcyRef { get; }
        IEnumerable<IAccountingElement> GetItemList();
        IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme);
        NodeType GetNodeType();
        IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, ICcyAsset ccyConv);
        IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, ICcyAsset ccyConv, string name);
        void ModifyAmount(FXMarket mkt, AssetMarket aMkt, string v, object valueAmount);
        void ModifyCcy(FXMarket mkt, AssetMarket aMkt, string v, ICcyAsset valueCcy, bool isLastRow);
        void Delete(string v); //Delete an item from the ItemList
    }
}
