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
        Presenter MainPresenter;

        public MainForm()
        {
            //XmlConfigurator.Configure();
            InitializeComponent();
            OnLoad();
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
            MainPresenter = new Presenter(this, Data);
            MainPresenter.LoadAccounts();
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

        public void ShowCategory(ICategory cat)
        {
            dataGridViewAccounting.ShowCategory(cat);
        }

        public void ShowInstitution(IInstitution cat)
        {
            dataGridViewAccounting.ShowInstitution(cat);
        }

        public void SetUpTree(Dictionary<string, Dictionary<string, List<string>>> sum)
        {
            TreeViewAccounting.SetUpTree(sum);
        }

        #endregion

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MainPresenter.TreeView_AfterSelect(sender, e);
        }

        private void NewToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Data.Reset();
            MainPresenter.LoadAccounts();
        }

        private void TreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            MainPresenter.TreeView_AfterLabelEdit(e);
            TreeViewAccounting.LabelEdit = false;
        }

        private void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            MainPresenter.TreeView_NodeMouseClick(e);
        }

        public void TreeView_NodeMouseRightClick(TreeNodeMouseClickEventArgs e)
        {
            TreeViewAccounting.NodeMouseRightClick(e);
        }
    }
}
