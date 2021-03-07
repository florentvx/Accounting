using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Core;
using Core.Interfaces;
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

        private void OnLoad()
        {
            MainPresenter = new Presenter(this, _DataHistory);
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

        protected bool StartPageCheck()
        {
            if (IsStartPageState)
            {
                MessageBox.Show("Create a new file or load a preexisting file.", "Error: No File Loaded");
                return false;
            }
            return true;
        }

        protected override void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StartPageCheck())
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
            bool test = false;
            using (OpenFileDialog form = new OpenFileDialog())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SetFilePath(form.FileName);
                    test = true;
                }
            }
            if (test)
            {
                HistoricalAccountingData desObject;
                using (StreamReader r = new StreamReader(FullPath))
                {
                    string json = r.ReadToEnd();
                    desObject = JsonConvert.DeserializeObject<HistoricalAccountingData>(json);
                }
                _DataHistory = desObject;
                _CurrentDate = _DataHistory.Dates.Last();
                MainPresenter.SetHistoricalData(_DataHistory);
                MainPresenter.LoadAccounts(showTotal: true);
            }
        }

        protected override void AddCurrencyToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            if (StartPageCheck())
            {
                using (AddCcyForm form = new AddCcyForm(Data.GetAvailableCurrencies()))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        MainPresenter.AddNewCcy(form.CcyName, form.CcyStatics, form.CcyPair, form.CcyPairQuote);
                        MainPresenter.LoadAccounts(specifiedAddress: _AddressofElementShowed);
                    }
                }
            }
        }

        protected override void AddAssetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StartPageCheck())
            {
                using (AddAssetForm form = new AddAssetForm(Data.GetAvailableCurrencies()))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        MainPresenter.AddNewAsset(form.AssetName, form.AssetStatics, form.AssetCcyPairQuote);
                        MainPresenter.LoadAccounts(specifiedAddress: _AddressofElementShowed);
                    }
                }
            }
        }

        protected override void AddDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StartPageCheck())
            {
                using (AddDateForm form = new AddDateForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                        MainPresenter.AddNewDate(form.GetDate());
                }
            }   
        }

        protected override void ApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutApplicationForm form = new AboutApplicationForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
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
            if (!IsStartPageState)
            {
                TabControl tc = (TabControl)sender;
                if (tc.SelectedTab.Name == "DataPage")
                    MainPresenter.LoadAccounts(true);
                if (tc.SelectedTab.Name == "GraphPage")
                    MainPresenter.UpdateChart();
                if (tc.SelectedTab.Name == "StaticsPage")
                    MainPresenter.UpdateStatics();
                if (tc.SelectedTab.Name == "SummaryPage")
                    MainPresenter.UpdateSummary();
            }
        }
    }
}
