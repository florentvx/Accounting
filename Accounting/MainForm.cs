using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Core;
using Core.Finance;
using Core.Interfaces;
using Core.Statics;
using Design;
using Design.SubForms;
using Newtonsoft.Json;

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
            //SetFilePath(@"\\New.json", startUp: true);

            //Category category = new Category("Banks", new Currency("USD"));
            //category.AddInstitution("Toto Bank");
            //category.AddAccount("Checking", "Toto Bank");
            //category.AddAccount("Saving", "Toto Bank");
            //Category category2 = new Category("Investing", new Currency("USD"));
            //category2.AddInstitution("Fidelity");
            //category2.AddAccount("ETF", "Fidelity");
            //category2.AddAccount("Bitcoin", "Fidelity");
            //List<Category> cats = new List<Category> { category, category2 };

            //List<Category> cats2 = new List<Category> { };
            //foreach (var item in cats)
            //{
            //    cats2.Add(item.Copy());
            //}

            //CurrencyAssetStaticsDataBase ccyDB = new CurrencyAssetStaticsDataBase();
            //ccyDB.AddCcy("USD", new CurrencyStatics("$", 3, 2));
            //ccyDB.AddCcy("EUR", new CurrencyStatics("€", 3, 2));
            //ccyDB.AddCcy("GBP", new CurrencyStatics("£", 3, 2));
            //ccyDB.AddCcy("JPY", new CurrencyStatics("¥", 4, 0));
            //ccyDB.RefCcy = new Currency("USD");
            //ccyDB.AddAsset("BTC", new AssetStatics("BTC", new Currency("USD")));
            //_DataHistory.SetCcyDB(ccyDB);
            

            //FXMarket market = new FXMarket(CcyDB.RefCcy);
            //market.AddQuote(new CurrencyPair(new Currency("EUR"), new Currency("USD")), 1.2);
            //market.AddQuote(new CurrencyPair(new Currency("GBP"), new Currency("USD")), 1.4);
            //market.AddQuote(new CurrencyPair(new Currency("USD"), new Currency("JPY")), 105.0);

            //FXMarket market2 = new FXMarket(CcyDB.RefCcy);
            //market2.AddQuote(new CurrencyPair(new Currency("EUR"), new Currency("USD")), 1.25);
            //market2.AddQuote(new CurrencyPair(new Currency("GBP"), new Currency("USD")), 1.5);
            //market2.AddQuote(new CurrencyPair(new Currency("USD"), new Currency("JPY")), 100.0);

            //AssetMarket aMarket = new AssetMarket();
            //aMarket.AddQuote(new AssetCcyPair(new Asset("BTC"), new Currency("USD")), 15000);
            //aMarket.PopulateWithFXMarket(market);

            //AssetMarket aMarket2 = new AssetMarket();
            //aMarket2.AddQuote(new AssetCcyPair(new Asset("BTC"), new Currency("USD")), 20000);
            //aMarket2.PopulateWithFXMarket(market2);

            //AccountingData ad1 = new AccountingData(cats, market, aMarket);
            //ad1.SetCcyDB(CcyDB);
            //AccountingData ad2 = new AccountingData(cats2, market2, aMarket2);
            //ad2.SetCcyDB(CcyDB);

            //AddAccountingData(DateTime.Today.AddMonths(-2), ad1);
            //AddAccountingData(DateTime.Today.AddMonths(-1), ad2);

            //FileStream fs = new FileStream(@"C:\\Users\flore\\OneDrive\\Documents\\temp\\test.xml", FileMode.OpenOrCreate);
            //System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(CurrencyAssetStaticsDataBase));
            //s.Serialize(fs, _DataHistory.CcyDB);

            //string fileName = @"C:\\Users\flore\\OneDrive\\Documents\\temp\\test.json";

            //string jsonString = JsonConvert.SerializeObject(_DataHistory.CcyDB, Formatting.Indented);
            //File.WriteAllText(fileName, jsonString);


        }

        private void OnLoad()
        {
            LoadTestData();
            MainPresenter = new Presenter(this, _DataHistory);
            //MainPresenter.LoadAccounts(showTotal: true);
        }

        protected override void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AddCcyRefForm form = new AddCcyRefForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _CurrentDate = form.GetDate();
                    MainPresenter.ResetAndAddRefCcy(CurrentDate, form.CcyName, form.CcyStatics);
                    MainPresenter.LoadAccounts();
                }
            }
        }

        protected override void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog form = new SaveFileDialog
            {
                DefaultExt = "json",
                Filter = "Json Files (*.json)|*.json",
                RestoreDirectory = false,
                CheckPathExists = true,
                CheckFileExists = false
            };
            if (form.ShowDialog() == DialogResult.OK)
            {
                string fullPath = form.FileName;
                SetFilePath(fullPath);
                SaveToolStripMenuItem_Click(sender, e);
            }
        }

        protected override void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_FilePath == "<NONE>")
                SaveAsToolStripMenuItem_Click(sender, e);
            else
            {
                string jsonString = JsonConvert.SerializeObject(_DataHistory, Formatting.Indented);
                File.WriteAllText(FullPath, jsonString);
                MessageBox.Show($"File Saved Under: {FullPath}", "File Saved");
            }
        }

        protected override void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog form = new OpenFileDialog())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SetFilePath(form.FileName);
                }
            }
            HistoricalAccountingData desObject;
            using (StreamReader r = new StreamReader(FullPath))
            {
                string json = r.ReadToEnd();
                desObject = JsonConvert.DeserializeObject<HistoricalAccountingData>(json);
            }
            _DataHistory = desObject;
            _CurrentDate = _DataHistory.Dates.Last();
            MainPresenter.SetHistoricalData(_DataHistory);
            MainPresenter.LoadAccounts();
        }

        protected override void AddCurrencyToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            using (AddCcyForm form = new AddCcyForm(Data.GetAvailableCurrencies()))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    MainPresenter.AddNewCcy(form.CcyName, form.CcyStatics, form.CcyPair, form.CcyPairQuote);
                    MainPresenter.LoadAccounts();
                }
            }   
        }

        protected override void AddAssetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AddAssetForm form = new AddAssetForm(Data.GetAvailableCurrencies()))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    MainPresenter.AddNewAsset(form.AssetName, form.AssetStatics, form.AssetCcyPairQuote);
                }
            }
        }

        protected override void AddDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AddDateForm form = new AddDateForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                    MainPresenter.AddNewDate(form.GetDate());
            }
        }

        protected override void ApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutApplicationForm form = new AboutApplicationForm())
            {
                if (form.ShowDialog() == DialogResult.OK) { }
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
                MainPresenter.LoadAccounts(true);
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

        protected override void MainTabControl_SelectedIndexChanged(object sender, EventArgs e) 
        {
            TabControl tc = (TabControl)sender;
            if (tc.SelectedTab.Name == "GraphPage")
                MainPresenter.UpdateChart();
        }
    }
}
