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

        #region Interface

        public void Reset()
        {
            TreeViewAccounting.Reset();
        }

        //public void ShowActive()
        //{
        //    dataGridViewAccounting.ShowActive();
        //}

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
                switch (na.NodeType)
                {
                    case NodeType.Category:
                        dataGridViewAccounting.ShowCategory(Data.GetCategory(na.Address[0]), Data.Map.GetElement(na));
                        break;
                    case NodeType.Institution:
                        dataGridViewAccounting.ShowInstitution(Data.GetInstitution(na.Address[0], na.Address[1]), Data.Map.GetElement(na));
                        break;
                    case NodeType.Account:
                        dataGridViewAccounting.ShowInstitution(Data.GetInstitution(na.Address[0], na.Address[1]), Data.Map.GetElement(na.GetParent()));
                        break;
                    default:
                        break;
                }
            }
        }

        public void ShowTotal()
        {
            labelTable.Text = "Total";
            dataGridViewAccounting.ShowTotal(Data);
        }

        public void ShowCategory(NodeAddress na)
        {
            dataGridViewAccounting.ShowCategory(Data.GetCategory(na), Data.Map.GetElement(na));
        }

        public void ShowInstitution(NodeAddress na)
        {
            dataGridViewAccounting.ShowInstitution(Data.GetInstitution(na), Data.Map.GetElement(na));
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

        #endregion

        virtual protected void NewToolStripMenuItem_Click(object sender, System.EventArgs e) { }
        virtual protected void ButtonTotal_Click(object sender, System.EventArgs e) { }

        #region TreeView Event Functions

        public void TreeView_NodeMouseLeftClick(TreeNodeMouseClickEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            labelTable.Text = na.GetLabelText();
            switch (na.NodeType)
            {
                case NodeType.Category:
                    ShowCategory(na);
                    break;

                case NodeType.Institution:
                    ShowInstitution(na);
                    break;
                case NodeType.Account:
                    ShowInstitution(na.GetParent());
                    break;
                default:
                    break;
            }
        }

        public void TreeView_NodeMouseRightClick(TreeNodeMouseClickEventArgs e)
        {
            TreeViewAccounting.NodeMouseRightClick(e);
        }

        public void TreeView_NodeAddition(object sender, TreeNodeMouseClickEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            NodeAddress newNode = Data.AddItem(na);
            SetUpTree(Data.Map);
            ChangeActive(newNode);
        }

        #endregion

        delegate void DelegateTree();
        delegate void DelegateTreeWithInput(TreeViewMapping tvm);
        delegate void DelegateTable(NodeAddress na);

        
    }
}
