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

        public void ShowActive()
        {
            dataGridViewAccounting.ShowActive();
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
                switch (na.NodeType)
                {
                    case NodeType.Category:
                        dataGridViewAccounting.ShowCategory(Data.GetCategory(na.Address[0]));
                        break;
                    case NodeType.Institution:
                    case NodeType.Account:
                        dataGridViewAccounting.ShowInstitution(Data.GetInstitution(na.Address[0], na.Address[1]));
                        break;
                    default:
                        break;
                }
            }
        }

        public void ShowCategory(ICategory cat)
        {
            dataGridViewAccounting.ShowCategory(cat);
        }

        public void ShowInstitution(IInstitution cat)
        {
            dataGridViewAccounting.ShowInstitution(cat);
        }

        public void SetUpTree()
        {
            if (TreeViewAccounting.InvokeRequired)
            {
                DelegateTree d = new DelegateTree(SetUpTree);
                this.Invoke(d, new object[] { });
            }
            else
                TreeViewAccounting.SetUpTree(Data.GetSummary());
        }

        public void SetUpTree(NodeAddress na)
        {
            if (TreeViewAccounting.InvokeRequired)
            {
                DelegateTreeWithInput d = new DelegateTreeWithInput(SetUpTree);
                this.Invoke(d, new object[] { na });
            }
            else
                TreeViewAccounting.SetUpTree(Data.GetSummary(), na);
        }

        #endregion

        public void TreeView_NodeMouseLeftClick(TreeNodeMouseClickEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            switch (na.NodeType)
            {
                case NodeType.Category:
                    ShowCategory(Data.GetCategory(na.Address[0]));
                    break;

                case NodeType.Institution:
                case NodeType.Account:
                    ShowInstitution(Data.GetInstitution(na.Address[0], na.Address[1]));
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
            SetUpTree(newNode);
            ChangeActive(newNode);
        }

        delegate void DelegateTree();
        delegate void DelegateTreeWithInput(NodeAddress na);
        delegate void DelegateTable(NodeAddress na);

    }
}
