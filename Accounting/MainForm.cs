using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Core;
using Core.Interfaces;
using Design;
//using log4net.Config;

namespace Accounting
{
    public partial class MainForm : FormAccounting, IView
    {
        Presenter MainPresenter;

        public MainForm() : base()
        {
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

        protected override void NewToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Data.Reset();
            MainPresenter.LoadAccounts();
        }

        protected override void ButtonTotal_Click(object sender, EventArgs e)
        {
            MainPresenter.ButtonTotal();
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
    }
}
