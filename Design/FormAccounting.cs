using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Core;
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

        public void ShowTotal()
        {
            labelTable.Text = "Total";
            dataGridViewAccounting.ShowTotal(Data);
        }

        public void ShowElement(NodeAddress na)
        {
            dataGridViewAccounting.ShowElement(Data.GetElement(na), Data.Map.GetElement(na));
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
        virtual protected void ButtonTotal_Click(object sender, System.EventArgs e) { }

        public void TreeView_NodeAddition(object sender, TreeNodeMouseClickEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            NodeAddress newNode = Data.AddItem(na);
            SetUpTree(Data.Map);
            ChangeActive(newNode);
        }

        delegate void DelegateTree();
        delegate void DelegateTreeWithInput(TreeViewMapping tvm);
        delegate void DelegateTable(NodeAddress na);

        private void TreeViewAccounting_AfterExpand(object sender, TreeViewEventArgs e)
        {
            Data.Map.GetElement((NodeAddress)e.Node.Tag).Expand = true;
        }
    }
}
