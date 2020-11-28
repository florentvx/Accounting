using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Finance;

namespace Core.Interfaces
{
    public interface IView
    {
        CurrencyStaticsDataBase CcyDB { get; }
        DateTime CurrentDate { get; }
        void Reset();
        void ChangeActive(NodeAddress nd);
        void ShowTotal();
        void ShowElement(NodeAddress tvme);
        void SetUpMarkets(CurrencyStaticsDataBase ccyDB, FXMarket mkt, AssetMarket aMkt);
        void SetUpTree(TreeViewMapping na);
        void TreeView_NodeMouseRightClick(TreeNodeMouseClickEventArgs e);
        void SetUpAccountingData(CurrencyStaticsDataBase ccyDb, IAccountingData ad);
        void TreeView_NodeMouseLeftClick(TreeNodeMouseClickEventArgs e);
        void UpdateDates();
    }
}
