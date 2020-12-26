﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using Core;
using Core.Finance;
using Core.Interfaces;
using Core.Statics;
using Design;

namespace Accounting
{
    public partial class FormAccounting : Form, IView
    {
        protected HistoricalAccountingData _DataHistory;
        protected DateTime? _CurrentDate;
        protected string _FilePath;
        protected string _FileName;

        public string FullPath { get { return _FilePath + _FileName + ".json"; } }

        public DateTime CurrentDate {
            get { return _CurrentDate.Value; }
            set { _CurrentDate = value; }
        }

        public void SetFilePath(string path, bool startUp = false)
        {
            string file = path.Split('\\').Last();
            _FileName = file.Split('.').First();
            Text = $"Accounting - {_FileName}";
            if (!startUp)
                _FilePath = path.Substring(0, path.Length - file.Length);
        }

        public CurrencyAssetStaticsDataBase CcyDB { get { return _DataHistory.CcyDB; } }
        public AccountingData Data { get { return _DataHistory.GetData(_CurrentDate.Value); } }

        public FormAccounting() : base()
        {
            InitializeComponent();
            _DataHistory = new HistoricalAccountingData();
            _CurrentDate = null;
            _FilePath = "<NONE>";
        }

        #region IView

        public void Reset()
        {
            TreeViewAccounting.Reset();
            //UpdateComboBoxDates();
            //_DataHistory.Clear();
            //_CurrentDate = DateTime.Today;
            //_DataHistory[_CurrentDate.Value] = new AccountingData();
        }

        public void UpdateDates()
        {
            UpdateComboBoxDates();
        }

        public void ChangeActive(NodeAddress na)
        {
            if (dataGridViewAccounting.InvokeRequired)
            {
                DelegateTable d = new DelegateTable(ChangeActive);
                this.Invoke(d, new object[] { na });
            }
            else
            {
                labelTable.Text = na.GetLabelText();
                if (na.NodeType == NodeType.Account)
                    na = na.GetParent();
                dataGridViewAccounting.ShowElement(Data.GetElement(na), Data.Map.GetElement(na));
            }
        }

        public void ShowActive()
        {
            dataGridViewAccounting.ShowActive();
        }

        public void ShowTotal()
        {
            labelTable.Text = "Total";
            dataGridViewAccounting.ShowTotal(Data);
        }

        public void ShowElement(NodeAddress na)
        {
            labelTable.Text = na.GetLabelText();
            dataGridViewAccounting.ShowElement(Data.GetElement(na), Data.Map.GetElement(na));
        }

        public void SetUpMarkets(CurrencyAssetStaticsDataBase ccyDB, FXMarket mkt, AssetMarket aMkt)
        {
            Data.SetCcyDB(ccyDB);
            dataGridViewAccounting.SetUpMarkets(ccyDB, mkt, aMkt);
            dataGridViewFXMarket.ShowMarket(mkt);
            dataGridViewAssetMarket.ShowMarket(aMkt);
        }

        public void SetUpTree(TreeViewMapping tvm)
        {
            if (TreeViewAccounting.InvokeRequired)
            {
                DelegateTreeWithInput d = new DelegateTreeWithInput(SetUpTree);
                this.Invoke(d, new object[] { tvm });
            }
            else
                TreeViewAccounting.SetUpTree(tvm);
        }

        public void SetUpAccountingData(CurrencyAssetStaticsDataBase ccyDB, IAccountingData iad)
        {
            SetUpMarkets(ccyDB, iad.FXMarket, iad.AssetMarket);
            SetUpTree(iad.Map);
        }

        public void TreeView_NodeMouseLeftClick(TreeNodeMouseClickEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            labelTable.Text = na.GetLabelText();
            switch (na.NodeType)
            {
                case NodeType.Category:
                case NodeType.Institution:
                    ShowElement(na);
                    break;
                case NodeType.Account:
                    ShowElement(na.GetParent());
                    break;
            }
        }

        public void TreeView_NodeMouseRightClick(TreeNodeMouseClickEventArgs e)
        {
            TreeViewAccounting.NodeMouseRightClick(e);
        }

        #endregion

        private void UpdateComboBoxDates()
        {
            ComboBoxDates.Items.Clear();
            int i = 0;
            foreach (DateTime item in _DataHistory.Dates)
            {
                ComboBoxDates.Items.Add(item.ToLongDateString());
                if (item == _CurrentDate)
                    ComboBoxDates.SelectedIndex = i;
                i++;
            }
        }

        public void AddAccountingData(DateTime date, AccountingData ad, bool selectNewDate = true)
        {
            _DataHistory.AddData(date, ad);   
            if (selectNewDate)
                _CurrentDate = date;
            UpdateComboBoxDates();
        }

        virtual protected void NewToolStripMenuItem_Click(object sender, System.EventArgs e) { }
        virtual protected void SaveAsToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void SaveToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void LoadToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void AddCurrencyToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void AddAssetToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void AddDateToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void ApplicationToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void ButtonTotal_Click(object sender, System.EventArgs e) { }
        virtual protected void ComboBoxDates_SelectedIndexChanged(object sender, EventArgs e) { }

        public void TreeView_NodeAddition(object sender, TreeNodeMouseClickEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            NodeAddress newNode = Data.AddItem(na);
            SetUpTree(Data.Map);
            ChangeActive(newNode);
        }

        public void TreeView_NodeDeletion(object sender, TreeNodeMouseClickEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            NodeAddress newActive = Data.DeleteItem(na);
            SetUpTree(Data.Map);
            ChangeActive(newActive);
        }

        delegate void DelegateTree();
        delegate void DelegateTreeWithInput(TreeViewMapping tvm);
        delegate void DelegateTable(NodeAddress na);

        private void TreeViewAccounting_AfterExpand(object sender, TreeViewEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            TreeViewMappingElement tvme = Data.Map.GetElement(na);
            tvme.Expand = true;
        }

        private void TreeViewAccounting_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            TreeViewMappingElement tvme = Data.Map.GetElement(na);
            tvme.Expand = false;
        }

        private void TreeViewAccounting_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = TreeViewAccounting.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = TreeViewAccounting.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            // Confirm that the node at the drop location is not 
            // the dragged node and that target node isn't null
            // (for example if you drag outside the control)
            if (!draggedNode.Equals(targetNode) && targetNode != null)
            {
                Data.Map.MoveNode((NodeAddress)draggedNode.Tag, (NodeAddress)targetNode.Tag);
                SetUpTree(Data.Map);
                TreeViewAccounting.ResetGraphics();
            }   
        }

        private void TreeViewAccounting_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void TreeViewAccounting_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            TreeViewAccounting.DefineRef(e);
        }

        private void TreeViewAccounting_DragOver(object sender, DragEventArgs e)
        {
            Point pt = TreeViewAccounting.PointToClient(new Point(e.X, e.Y));
            TreeViewAccounting.ShowLine(pt);
        }

        private void DataGridViewFXMarket_ValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Currency ccy1 = new Currency(dataGridViewFXMarket.Rows[e.RowIndex].Cells[DataGridViewMarketStatics.Column_Asset1].Value);
            Currency ccy2 = new Currency(dataGridViewFXMarket.Rows[e.RowIndex].Cells[DataGridViewMarketStatics.Column_Asset2].Value);
            double rate = Convert.ToDouble(dataGridViewFXMarket.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            switch (e.ColumnIndex)
            {
                case DataGridViewMarketStatics.Column_Value:
                    Data.FXMarket.AddQuote(new CurrencyPair(ccy1, ccy2), rate);
                    Data.UpdateAssetMarket();
                    ShowActive();
                    break;
            }
        }

        private void DataGridViewAssetMarket_ValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Asset asset = new Asset(dataGridViewAssetMarket.Rows[e.RowIndex].Cells[DataGridViewMarketStatics.Column_Asset1].Value);
            Currency ccy2 = new Currency(dataGridViewAssetMarket.Rows[e.RowIndex].Cells[DataGridViewMarketStatics.Column_Asset2].Value);
            double rate = Convert.ToDouble(dataGridViewAssetMarket.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            switch (e.ColumnIndex)
            {
                case DataGridViewMarketStatics.Column_Value:
                    Data.AssetMarket.AddQuote(new AssetCcyPair(asset, ccy2), rate);
                    Data.UpdateAssetMarket();
                    ShowActive();
                    break;
            }
        }
    }
}
