using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Core;
using Core.Interfaces;
using log4net.Config;

namespace Accounting
{
    public partial class MainForm : Form, IView
    {
        AccountingData Data;

        public MainForm()
        {
            XmlConfigurator.Configure();
            InitializeComponent();
            OnLoad();
        }

        private void LoadAccounts()
        {
            TreeView.Nodes.Clear();
            foreach (Category itemC in Data)
            {
                TreeNode treeNodeC = new TreeNode(itemC.CategoryName);
                foreach (var itemI in itemC.Insitutions)
                {
                    TreeNode treeNodeI = new TreeNode(itemI.InstitutionName);
                    foreach (var itemA in itemI.Accounts)
                    {
                        TreeNode treeNodeA = new TreeNode(itemA.AccountName);
                        treeNodeI.Nodes.Add(treeNodeA);
                    }
                    treeNodeC.Nodes.Add(treeNodeI);
                }
                TreeView.Nodes.Add(treeNodeC);
            }
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

        private void SetUpTable()
        {
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "Account Name";
            dataGridView1.Columns[1].Name = "Currency";
            dataGridView1.Columns[2].Name = "Amount";
            dataGridView1.Columns[3].Name = "Converted Amount";
        }

        private void ShowInstitution(string catName, string institutionName)
        {
            dataGridView1.Rows.Clear();
            Institution instit = Data.GetInstitution(catName, institutionName);
            foreach (Account item in instit.Accounts)
            {
                dataGridView1.Rows.Add(
                    item.AccountName,
                    item.Ccy.ToString(),
                    item.Amount,
                    item.Amount);
            }
        }

        private void OnLoad()
        {
            SetUpTable();
            LoadTestData();
            LoadAccounts();
            ShowInstitution("Banks", "Toto Bank");
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string fullPath = e.Node.FullPath;
            string[] split = fullPath.Split('\\');
            if (split.Count() == 2)
            {
                ShowInstitution(split[0], split[1]);
                e.Node.Expand();
            }
        }
    }
}
