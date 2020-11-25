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

            List<Category> cats2 = new List<Category> { };
            foreach (var item in cats)
            {
                cats2.Add(item.Copy());
            }

            CurrencyStaticsDataBase ccyDB = new CurrencyStaticsDataBase();
            ccyDB.AddCcy("USD", new CurrencyStatics("$", 3, 2));
            ccyDB.AddCcy("EUR", new CurrencyStatics("€", 3, 2));
            ccyDB.AddCcy("GBP", new CurrencyStatics("£", 3, 2));
            ccyDB.AddCcy("JPY", new CurrencyStatics("¥", 4, 0));

            FXMarket market = new FXMarket(ccyDB);
            market.AddQuote(new CurrencyPair(new Currency("EUR"), new Currency("USD")), 1.2);
            market.AddQuote(new CurrencyPair(new Currency("GBP"), new Currency("USD")), 1.4);
            market.AddQuote(new CurrencyPair(new Currency("USD"), new Currency("JPY")), 105.0);

            FXMarket market2 = new FXMarket(ccyDB);
            market2.AddQuote(new CurrencyPair(new Currency("EUR"), new Currency("USD")), 1.25);
            market2.AddQuote(new CurrencyPair(new Currency("GBP"), new Currency("USD")), 1.5);
            market2.AddQuote(new CurrencyPair(new Currency("USD"), new Currency("JPY")), 100.0);

            AssetMarket aMarket = new AssetMarket();
            aMarket.AddQuote(new AssetCcyPair(new Asset("BTC"), new Currency("USD")), 15000);
            aMarket.PopulateWithFXMarket(market);

            AssetMarket aMarket2 = new AssetMarket();
            aMarket2.AddQuote(new AssetCcyPair(new Asset("BTC"), new Currency("USD")), 20000);
            aMarket2.PopulateWithFXMarket(market2);

            AddAccountingData(DateTime.Today.AddMonths(-1), new AccountingData(cats, market, aMarket));
            AddAccountingData(DateTime.Today, new AccountingData(cats2, market2, aMarket2));
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

        protected override void ComboBoxDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CurrentDate = DateTime.Parse(ComboBoxDates.SelectedItem.ToString());
            if (MainPresenter != null)
            {
                MainPresenter.SetAccountingData(Data);
                MainPresenter.LoadAccounts();
            }
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
