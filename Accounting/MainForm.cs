using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Core;
using Core.Finance;
using Core.Interfaces;
using Design;
using Design.SubForm;

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
            Category category = new Category("Banks", new Currency("USD"));
            category.AddInstitution("Toto Bank");
            category.AddAccount("Checking", "Toto Bank");
            category.AddAccount("Saving", "Toto Bank");
            Category category2 = new Category("Investing", new Currency("USD"));
            category2.AddInstitution("Fidelity");
            category2.AddAccount("ETF", "Fidelity");
            category2.AddAccount("Bitcoin", "Fidelity");
            List<Category> cats = new List<Category> { category, category2 };

            CurrencyStaticsDataBase ccyDB = new CurrencyStaticsDataBase();
            ccyDB.AddCcy("USD", new CurrencyStatics("$", 3, 2));
            ccyDB.AddCcy("EUR", new CurrencyStatics("€", 3, 2));
            ccyDB.AddCcy("GBP", new CurrencyStatics("£", 3, 2));
            ccyDB.AddCcy("JPY", new CurrencyStatics("¥", 4, 0));

            FXMarket market = new FXMarket(ccyDB);
            market.AddQuote(new CurrencyPair(new Currency("EUR"), new Currency("USD")), 1.2);
            market.AddQuote(new CurrencyPair(new Currency("GBP"), new Currency("USD")), 1.4);
            market.AddQuote(new CurrencyPair(new Currency("USD"), new Currency("JPY")), 105.0);

            AssetMarket aMarket = new AssetMarket();
            aMarket.AddQuote(new AssetCcyPair(new Asset("BTC"), new Currency("USD")), 15000);
            aMarket.PopulateWithFXMarket(market);

            Data = new AccountingData(cats, market, aMarket);
        }

        private void OnLoad()
        {
            LoadTestData();
            MainPresenter = new Presenter(this, Data);
            MainPresenter.LoadAccounts();
        }

        protected override void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AddCcyRefForm form = new AddCcyRefForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    MainPresenter.AddRefCcy(form.CcyName, form.CcyStatics);
                    MainPresenter.LoadAccounts();
                }
            }
        }

        protected override void AddCurrencyToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            using (AddCcyForm form = new AddCcyForm(Data.GetAvailableCurrencies()))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    MainPresenter.AddNewCcy(form.CcyName, form.CcyStatics, form.CcyPair, form.CcyPairQuote);
                }
            }   
        }

        protected override void AddAssetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AddAssetForm form = new AddAssetForm(Data.GetAvailableCurrencies()))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    MainPresenter.AddNewAsset(form.AssetName, form.AssetCcyPair, form.AssetCcyPairQuote);
                }
            }
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
