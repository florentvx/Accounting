using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Core;
using Core.Finance;
using Core.Interfaces;
using Design;

namespace Accounting
{
    public partial class FormAccounting : Form, IView
    {
        protected AccountingData Data;

        public FormAccounting() : base()
        {
            InitializeComponent();
        }

        #region IView

        public void Reset()
        {
            TreeViewAccounting.Reset();
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

        public void SetUpMarket(Market mkt)
        {
            dataGridViewAccounting.SetUpMarket(mkt);
            dataGridViewMarket.ShowMarket(mkt);
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

        virtual protected void NewToolStripMenuItem_Click(object sender, System.EventArgs e) { }
        virtual protected void AddQuoteToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void ButtonTotal_Click(object sender, System.EventArgs e) { }

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

        private void DataGridViewMarket_ValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Currency ccy1 = new Currency(dataGridViewMarket.Rows[e.RowIndex].Cells[DataGridViewMarketStatics.Column_Asset1].Value);
            Currency ccy2 = new Currency(dataGridViewMarket.Rows[e.RowIndex].Cells[DataGridViewMarketStatics.Column_Asset2].Value);
            double rate = Convert.ToDouble(dataGridViewMarket.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            switch (e.ColumnIndex)
            {
                case DataGridViewMarketStatics.Column_Value:
                    Data.Market.AddQuote(new CurrencyPair(ccy1, ccy2), rate);
                    ShowActive();
                    break;
            }
        }
    }
}
