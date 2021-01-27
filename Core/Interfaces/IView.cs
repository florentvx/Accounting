using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Finance;
using Core.Statics;

namespace Core.Interfaces
{
    public interface IView
    {
        CurrencyAssetStaticsDataBase CcyDB { get; }
        DateTime CurrentDate { get; set; }
        void Reset();
        void ChangeActive(NodeAddress nd);
        void ShowTotal();
        void ShowElement(NodeAddress tvme);
        void SetUpMarkets(CurrencyAssetStaticsDataBase ccyDB, FXMarket mkt, AssetMarket aMkt);
        void SetUpTree(TreeViewMapping na);
        void TreeView_NodeMouseRightClick(TreeNodeMouseClickEventArgs e);
        void SetUpAccountingData(CurrencyAssetStaticsDataBase ccyDb, IAccountingData ad);
        void TreeView_NodeMouseLeftClick(TreeNodeMouseClickEventArgs e);
        void UpdateDates();
        void Chart_Update();
        void Statics_Update();

    }
}
