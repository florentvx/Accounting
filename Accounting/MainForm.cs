using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Core;
using Core.Interfaces;
//using log4net.Config;

namespace Accounting
{
    public partial class MainForm : Form, IView
    {
        AccountingData Data;

        public MainForm()
        {
            //XmlConfigurator.Configure();
            InitializeComponent();
            OnLoad();
        }

        private void LoadAccounts()
        {
            TreeView.Nodes.Clear();
            var add = Data.GetSummary();
            foreach (var itemC in add)
            {
                TreeNode treeNodeC = new TreeNode(itemC.Key);
                foreach (var itemI in itemC.Value)
                {
                    TreeNode treeNodeI = new TreeNode(itemI.Key);
                    foreach (string itemA in itemI.Value)
                    {
                        TreeNode treeNodeA = new TreeNode(itemA);
                        treeNodeI.Nodes.Add(treeNodeA);
                    }
                    treeNodeC.Nodes.Add(treeNodeI);
                }
                TreeView.Nodes.Add(treeNodeC);
            }
            dataGridViewAccounting.ShowCategory(Data.GetFirstCategory());
        }

        private void LoadTestData()
        {
            Category category = new Category("Banks");
            category.AddInstitution("Toto Bank");
            category.AddAccount("Checking", "Toto Bank");
            category.AddAccount("Saving", "Toto Bank");
            Category category2 = new Category("Investing");
            category2.AddInstitution("Fidelity");
            category2.AddAccount("ETF", "Fidelity");
            List<Category> cats = new List<Category> { category, category2 };
            Data = new AccountingData(cats);
        }

        private void OnLoad()
        {
            LoadTestData();
            LoadAccounts();
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string fullPath = e.Node.FullPath;
            string[] split = fullPath.Split('\\');
            if (split.Count() == 1)
            {
                dataGridViewAccounting.ShowCategory(Data, split[0]);
                e.Node.Expand();
            }
            if (split.Count() == 2)
            {
                dataGridViewAccounting.ShowInstitution(Data, split[0], split[1]);
                e.Node.Expand();
            }
        }

        private void NewToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Data.Reset();
            LoadAccounts();
        }
    }
}
