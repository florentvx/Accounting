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

        private void SetUpTable()
        {
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "Account Name";
            dataGridView1.Columns[1].Name = "Currency";
            dataGridView1.Columns[2].Name = "Amount";
            dataGridView1.Columns[3].Name = "Converted Amount";
        }

        #region Show Institution

        private void AddRow(Account item, bool isTotal = false)
        {
            DataGridViewRow dgvr = new DataGridViewRow();
            dgvr.CreateCells(dataGridView1);
            string amount = item.Amount.ToString();
            if (isTotal)
                amount = "";
            var titles = new object[] {
                item.AccountName, item.Ccy.ToString(), amount, item.Amount
            };
            dgvr.SetValues(titles);
            if (isTotal)
            {
                dgvr.ReadOnly = isTotal;
                for (int i = 0; i < dgvr.Cells.Count; i++)
                {
                    dgvr.Cells[i].Style.BackColor = Color.LightGray;
                }   
            }
            dataGridView1.Rows.Add(dgvr);
        }

        private void ShowInstitution(Institution instit)
        {
            dataGridView1.Rows.Clear();
            foreach (Account item in instit.Accounts)
                AddRow(item);            
            AddRow(instit.TotalAccount(), isTotal: true);
        }

        private void ShowInstitution(string catName, string institName)
        {
            ShowInstitution(Data.GetInstitution(catName, institName));
        }

        #endregion

        #region Show Category

        private void AddRow(Institution item)
        {
            Account sum = item.TotalAccount();
            dataGridView1.Rows.Add(
                    item.InstitutionName,
                    item.Ccy.ToString(),
                    sum.Amount,
                    sum.Amount);
        }

        private void ShowCategory(Category cat)
        {
            dataGridView1.Rows.Clear();
            foreach (Institution item in cat.Institutions)
                AddRow(item);
            AddRow(cat.TotalInstitution(),isTotal: true);
        }

        private void ShowCategory(string catName)
        {
            ShowCategory(Data.GetCategory(catName));
        }

        #endregion

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
            ShowCategory(Data.GetFirstCategory());
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
            SetUpTable();
            LoadTestData();
            LoadAccounts();
            dataGridView1.AllowUserToAddRows = false;
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string fullPath = e.Node.FullPath;
            string[] split = fullPath.Split('\\');
            if (split.Count() == 1)
            {
                ShowCategory(split[0]);
                e.Node.Expand();
            }
            if (split.Count() == 2)
            {
                ShowInstitution(split[0], split[1]);
                e.Node.Expand();
            }
        }

        private void NewToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Data.Reset();
            LoadAccounts();
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == dataGridView1.RowCount - 1)
                dataGridView1.ClearSelection();
        }
    }
}
