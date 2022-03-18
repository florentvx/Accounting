using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Core;
using Core.Finance;
using Core.Interfaces;
using Core.Statics;
using Design;

namespace Accounting
{
    public partial class FormAccounting : Form, IView
    {
        private string AppName = "SortFin";

        protected HistoricalAccountingData _DataHistory;
        protected DateTime? _CurrentDate;
        protected NodeAddress _AddressofElementShowed;

        protected string _FilePath;
        protected string _FileName;

        public string FullPath { get { return _FilePath + _FileName + ".json"; } }

        public DateTime CurrentDate {
            get { return _CurrentDate.Value; }
            set { _CurrentDate = value; }
        }

        public DateTime? GetPreviousDate()
        {
            var res = _DataHistory.Dates.Where(x => x < CurrentDate).LastOrDefault();
            if (_DataHistory.Dates.Contains(res))
                return res;
            else
                return null;
        }

        public bool IsStartPageState { get { return !_CurrentDate.HasValue; } }

        public void SetFilePath(string path, bool startUp = false)
        {
            string file = path.Split('\\').Last();
            _FileName = file.Split('.').First();
            Text = $"{AppName} - {_FileName}";
            if (!startUp)
                _FilePath = path.Substring(0, path.Length - file.Length);
        }

        public CurrencyAssetStaticsDataBase CcyDB { get { return _DataHistory.CcyDB; } }
        public AccountingData Data { get { return _DataHistory.GetData(_CurrentDate.Value); } }

        public FormAccounting() : base()
        {
            InitializeComponent();
            _DataHistory = new HistoricalAccountingData();
            _CurrentDate = null;
            _FilePath = "<NONE>";
            dataGridViewAccounting.SetUpTable();
            dataGridViewFXMarket.SetUpTable();
            dataGridViewAssetMarket.SetUpTable();
        }

        #region IView

        public void Reset()
        {
            TreeViewAccounting.Reset();
            //UpdateComboBoxDates();
            //_DataHistory.Clear();
            //_CurrentDate = DateTime.Today;
            //_DataHistory[_CurrentDate.Value] = new AccountingData();
        }

        public void UpdateDates()
        {
            UpdateComboBoxDates();
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
                labelTable.Text = na.GetLabelText();
                if (na.NodeType == NodeType.Account)
                    na = na.GetParent();
                dataGridViewAccounting.ShowElement(Data.GetElement(na), Data.Map.GetElement(na));
            }
        }

        public void ShowActive()
        {
            dataGridViewAccounting.ShowActive();
        }

        public void ShowTotal()
        {
            labelTable.Text = "Total";
            DateTime? dt = GetPreviousDate();
            Dictionary<string, Price> lastTotal = new Dictionary<string, Price> { };
            if (dt.HasValue)
            {
                AccountingData lastData = _DataHistory.GetData(dt.Value);
                lastTotal[dataGridViewAccounting._LastTotalMemoryMainKey] = lastData.GetTotalPrice(_DataHistory.TotalCcy);
                foreach (var cat in lastData.Categories)
                    lastTotal[cat.CategoryName] = cat.GetTotalAmount(_DataHistory.TotalCcy, lastData.FXMarket);
                dataGridViewAccounting.SetPreviousFXMarket(lastData.FXMarket);
            }
            
            dataGridViewAccounting.ShowTotal(Data, lastTotal);
        }

        public void ShowTotal(object sender, EventArgs e)
        {
            ShowTotal();
        }

        public void ShowElement(NodeAddress na)
        {
            labelTable.Text = na.GetLabelText();
            DateTime? dt = GetPreviousDate();
            IAccountingElement element = Data.GetElement(na);
            Dictionary<string, Price> lastTotal = new Dictionary<string, Price> { };
            if (dt.HasValue)
            {
                AccountingData lastData = _DataHistory.GetData(dt.Value);
                lastTotal[dataGridViewAccounting._LastTotalMemoryMainKey] = lastData.GetTotalPrice(_DataHistory.TotalCcy, na);
                //lastTotal[dataGridViewAccounting._LastTotalMemoryMainKey] = lastData.GetQuote(lastData.Ccy, _DataHistory.TotalCcy) * lastData.GetValue(na);
                foreach (var subitem in lastData.GetElement(na).GetItemList())
                    lastTotal[subitem.GetName()] = subitem.GetTotalAmount(_DataHistory.TotalCcy, lastData.FXMarket);
            }
            dataGridViewAccounting.ShowElement(Data.GetElement(na), Data.Map.GetElement(na), lastTotal); ;
            _AddressofElementShowed = na;
        }

        public void SetUpMarkets(CurrencyAssetStaticsDataBase ccyDB, FXMarket mkt, AssetMarket aMkt, FXMarket prevMkt)
        {
            _DataHistory.SetCcyDB(ccyDB);
            dataGridViewAccounting.SetUpMarkets(ccyDB, mkt, aMkt, prevMkt);
            dataGridViewFXMarket.ShowMarket(mkt, ccyDB);
            dataGridViewAssetMarket.ShowMarket(aMkt, ccyDB);
        }

        public void SetUpTree(TreeViewMapping tvm)
        {
            if (TreeViewAccounting.InvokeRequired)
            {
                DelegateTreeWithInput d = new DelegateTreeWithInput(SetUpTree);
                this.Invoke(d, new object[] { tvm });
            }
            else
                TreeViewAccounting.SetUpTree(tvm);
        }

        public void SetUpAccountingData(CurrencyAssetStaticsDataBase ccyDB, IAccountingData iad)
        {
            SetUpMarkets(ccyDB, iad.FXMarket, iad.AssetMarket, null);
            SetUpTree(iad.Map);
        }

        public void TreeView_NodeMouseLeftClick(TreeNodeMouseClickEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            labelTable.Text = na.GetLabelText();
            switch (na.NodeType)
            {
                case NodeType.Category:
                case NodeType.Institution:
                    ShowElement(na);
                    break;
                case NodeType.Account:
                    ShowElement(na.GetParent());
                    break;
            }
        }

        public void TreeView_NodeMouseRightClick(TreeNodeMouseClickEventArgs e)
        {
            TreeViewAccounting.NodeMouseRightClick(e);
        }

        #endregion

        #region Chart

        readonly string ChartNone = "None";
        readonly string ChartAllCategories = "<All Categories>";
        readonly string ChartAllInstitutions = "<All Institutions>";
        readonly string ChartAllAccounts = "<All Accounts>";

        private void GraphUpdate()
        {
            Chart.Series.Clear();
            NodeAddress na = Chart_GetNodeAddress();
            Series ser = new Series("Value") { XValueType = ChartValueType.String };
            List<double> values = new List<double> { };
            foreach (var item in _DataHistory.Data)
            {
                Price val = item.Value.GetValue(na);
                values.Add(val.Value);
                ser.Points.AddXY(item.Key, val);
            }
            Chart.Series.Add(ser);
            Chart.Series[0].ChartType = SeriesChartType.Line;
            double min = Math.Round(Math.Min(values.Min() * 0.9, values.Min() - 100));
            double max = Math.Round(Math.Max(values.Max() * 1.1, values.Max() + 100));
            Chart.ChartAreas[0].AxisY.Minimum = min;
            Chart.ChartAreas[0].AxisY.Maximum = max;
            Chart.ChartAreas[0].AxisY.CustomLabels.Clear();
            double n = 8.0;
            int decNb = _DataHistory.CcyDB.GetCcyStatics(_DataHistory.TotalCcy).DecimalNumber;
            Chart.ChartAreas[0].AxisY.MajorGrid.Interval = (max - min) / n;
            Chart.ChartAreas[0].AxisY.MajorGrid.IntervalOffset = (max - min) / n / 2.0;
            Chart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
            for (int i = 0; i < n; i++)
            {
                double min_i = min + (max - min) / n * i;
                double max_i = min + (max - min) / n * (i + 1);
                double value_i = Math.Round(0.5 * (min_i + max_i) * Math.Pow(10, -4 + decNb)) * Math.Pow(10, 4 - decNb);
                string valueStr_i = _DataHistory.CcyDB.CcyToString(_DataHistory.TotalCcy, value_i);
                Chart.ChartAreas[0].AxisY.CustomLabels.Add(new CustomLabel(min_i, max_i, valueStr_i, 0, LabelMarkStyle.None));
            }
            Chart.Series[0].XValueType = ChartValueType.DateTime;
            Chart.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
            Chart.ChartAreas[0].AxisX.Interval = 3;
            Chart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
            Chart.ChartAreas[0].AxisX.IntervalOffset = 1;
            Chart.Series[0].BorderWidth = 2;
        }

        public NodeAddress Chart_GetNodeAddress()
        {
            string path = "";
            string categorySelected = (string)comboBoxGraphCategory.SelectedItem;
            if (categorySelected == ChartAllCategories)
                return new NodeAddress(NodeType.All, path);
            else
            {
                path += categorySelected;
                string institSelected = (string)comboBoxGraphInstitution.SelectedItem;
                if (institSelected == ChartAllInstitutions)
                    return new NodeAddress(NodeType.Category, path);
                else
                {
                    path += NodeAddress.Separator + institSelected;
                    string accountSelected = (string)comboBoxGraphAccount.SelectedItem;
                    if (accountSelected == ChartAllAccounts)
                        return new NodeAddress(NodeType.Institution, path);
                    else
                    {
                        path += NodeAddress.Separator + accountSelected;
                        return new NodeAddress(NodeType.Account, path);
                    }
                }
            }
        }

        public void Chart_ResetComboBoxAccount()
        {
            comboBoxGraphAccount.Items.Clear();
            string selectedCat = (string)comboBoxGraphCategory.SelectedItem;
            string selectedInstit = (string)comboBoxGraphInstitution.SelectedItem;
            bool Activate = selectedInstit != ChartAllInstitutions
                            && selectedInstit != ChartNone;
            if (Activate)
            {
                comboBoxGraphAccount.Items.Add(ChartAllAccounts);
                NodeAddress na = new NodeAddress(   NodeType.Institution, 
                                                    selectedCat + NodeAddress.Separator + selectedInstit);
                var subNodes = Data.Map.GetSubNodes(na);
                foreach (var item in subNodes)
                {
                    comboBoxGraphAccount.Items.Add(item);
                }
                comboBoxGraphAccount.SelectedItem = ChartAllAccounts;
            }
            else
            {
                comboBoxGraphAccount.Items.Add(ChartNone);
                comboBoxGraphAccount.SelectedItem = ChartNone;
            }
        }

        public void Chart_ResetComboBoxInstitution()
        {
            comboBoxGraphInstitution.Items.Clear();
            string selectedCat = (string)comboBoxGraphCategory.SelectedItem;
            bool Activate = selectedCat != ChartAllCategories;
            if (Activate)
            {
                comboBoxGraphInstitution.Items.Add(ChartAllInstitutions);
                NodeAddress na = new NodeAddress(NodeType.Category, selectedCat);
                var subNodes = Data.Map.GetSubNodes(na);
                foreach (var item in subNodes)
                {
                    comboBoxGraphInstitution.Items.Add(item);
                }
                comboBoxGraphInstitution.SelectedItem = ChartAllInstitutions;
            }
            else
            {
                comboBoxGraphInstitution.Items.Add(ChartNone);
                comboBoxGraphInstitution.SelectedItem = ChartNone;
            }
        }

        private void ComboBoxGraphCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            Chart_ResetComboBoxInstitution();
            GraphUpdate();
        }

        private void ComboBoxGraphInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            Chart_ResetComboBoxAccount();
            GraphUpdate();
        }

        private void comboBoxGraphAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            GraphUpdate();
        }

        public void Chart_Update()
        {
            // Combo Box - Total Ccy
            comboBoxGraphTotalCcy.Items.Clear();
            foreach (var item in _DataHistory.CcyDB.DataBase)
            {
                comboBoxGraphTotalCcy.Items.Add(item.Name);
            }
            comboBoxGraphTotalCcy.SelectedItem = _DataHistory.TotalCcy.CcyString;

            // Combo Box - Category
            comboBoxGraphCategory.Items.Clear();
            comboBoxGraphCategory.Items.Add(ChartAllCategories);
            var subNodes = Data.Map.GetSubNodes(new NodeAddress(NodeType.All, ""));
            foreach (var item in subNodes)
            {
                comboBoxGraphCategory.Items.Add(item);
            }
            comboBoxGraphCategory.SelectedItem = ChartAllCategories;

            // Combo Boxes
            Chart_ResetComboBoxInstitution();
            Chart_ResetComboBoxAccount();

            // Charts
            GraphUpdate();
        }

        #endregion

        public void Statics_Update()
        {
            dataGridViewStaticsCcy.Update(CcyDB);
            dataGridViewStaticsAsset.Update(CcyDB);
        }


        private void UpdateComboBoxDates()
        {
            ComboBoxDates.Items.Clear();
            int i = 0;
            foreach (DateTime item in _DataHistory.Dates)
            {
                ComboBoxDates.Items.Add(item.ToLongDateString());
                if (item == _CurrentDate)
                    ComboBoxDates.SelectedIndex = i;
                i++;
            }
        }

        public void AddAccountingData(DateTime date, AccountingData ad, bool selectNewDate = true)
        {
            _DataHistory.AddData(date, ad);   
            if (selectNewDate)
                _CurrentDate = date;
            UpdateComboBoxDates();
        }

        virtual protected void NewToolStripMenuItem_Click(object sender, System.EventArgs e) { }
        virtual protected void SaveAsToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void SaveToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void LoadToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void AddCurrencyToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void AddAssetToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void AddDateToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void ApplicationToolStripMenuItem_Click(object sender, EventArgs e) { }
        virtual protected void ButtonTotal_Click(object sender, System.EventArgs e) { }
        virtual protected void ComboBoxDates_SelectedIndexChanged(object sender, EventArgs e) { }
        virtual protected void MainTabControl_SelectedIndexChanged(object sender, EventArgs e) { }

        #region TreeViewAccounting

        public void TreeView_NodeAddition(object sender, TreeNodeMouseClickEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            NodeAddress newNode = Data.AddItem(na);
            SetUpTree(Data.Map);
            ChangeActive(newNode);
        }

        public void TreeView_NodeDeletion(object sender, TreeNodeMouseClickEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            NodeAddress newActive = Data.DeleteItem(na);
            SetUpTree(Data.Map);
            ChangeActive(newActive);
        }

        delegate void DelegateTree();
        delegate void DelegateTreeWithInput(TreeViewMapping tvm);
        delegate void DelegateTable(NodeAddress na);

        private void TreeViewAccounting_AfterExpand(object sender, TreeViewEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            TreeViewMappingElement tvme = Data.Map.GetElement(na);
            tvme.Expand = true;
        }

        private void TreeViewAccounting_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            NodeAddress na = (NodeAddress)e.Node.Tag;
            TreeViewMappingElement tvme = Data.Map.GetElement(na);
            tvme.Expand = false;
        }

        private void TreeViewAccounting_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = TreeViewAccounting.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = TreeViewAccounting.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            // Confirm that the node at the drop location is not 
            // the dragged node and that target node isn't null
            // (for example if you drag outside the control)
            if (!draggedNode.Equals(targetNode) && targetNode != null)
            {
                Data.Map.MoveNode((NodeAddress)draggedNode.Tag, (NodeAddress)targetNode.Tag);
                SetUpTree(Data.Map);
                TreeViewAccounting.ResetGraphics();
            }   
        }

        private void TreeViewAccounting_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void TreeViewAccounting_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            TreeViewAccounting.DefineRef(e);
        }

        private void TreeViewAccounting_DragOver(object sender, DragEventArgs e)
        {
            Point pt = TreeViewAccounting.PointToClient(new Point(e.X, e.Y));
            TreeViewAccounting.ShowLine(pt);
        }

        #endregion

        #region DataGridViewMarket

        private void DataGridViewFXMarket_ValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Currency ccy1 = new Currency(dataGridViewFXMarket.Rows[e.RowIndex].Cells[DataGridViewMarketStatics.Column_Asset1].Value);
            Currency ccy2 = new Currency(dataGridViewFXMarket.Rows[e.RowIndex].Cells[DataGridViewMarketStatics.Column_Asset2].Value);
            object rateStr = dataGridViewFXMarket.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            try
            {
                double rate = Convert.ToDouble(rateStr);
                switch (e.ColumnIndex)
                {
                    case DataGridViewMarketStatics.Column_Value:
                        Data.FXMarket.AddQuote(new CurrencyPair(ccy1, ccy2), rate);
                        Data.UpdateAssetMarket();
                        Data.RefreshTotalAmount(Data.FXMarket, Data.AssetMarket);
                        ShowActive();
                        break;
                }
            }
            catch
            {
                MessageBox.Show($"The new rate input is not valid: {rateStr}", "Ccy Market Error");
                dataGridViewFXMarket.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Data.FXMarket.GetQuote(new CurrencyPair(ccy1, ccy2));
            }
            
        }

        private void DataGridViewAssetMarket_ValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Asset asset = new Asset(dataGridViewAssetMarket.Rows[e.RowIndex].Cells[DataGridViewMarketStatics.Column_Asset1].Value);
            Currency ccy2 = new Currency(dataGridViewAssetMarket.Rows[e.RowIndex].Cells[DataGridViewMarketStatics.Column_Asset2].Value);
            object rateStr = dataGridViewAssetMarket.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            try
            {
                double rate = Convert.ToDouble(rateStr);
                switch (e.ColumnIndex)
                {
                    case DataGridViewMarketStatics.Column_Value:
                        Data.AssetMarket.AddQuote(new AssetCcyPair(asset, ccy2), rate);
                        Data.UpdateAssetMarket();
                        Data.RefreshTotalAmount(Data.FXMarket, Data.AssetMarket);
                        ShowActive();
                        break;
                }
            }
            catch
            {
                MessageBox.Show($"The new rate input is not valid: {rateStr}", "Asset Market Error");
                dataGridViewFXMarket.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Data.AssetMarket.GetQuote(new AssetCcyPair(asset, ccy2));
            }
        }

        #endregion

        #region Graph

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            HitTestResult result = Chart.HitTest(e.X, e.Y);
            // Reset Data Point Attributes
            foreach (DataPoint point in Chart.Series[0].Points)
            {
                point.MarkerStyle = MarkerStyle.None;
                point.Color = Color.FromArgb(200, 30, 144, 255);
            }
            // If the mouse if over a data point
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                // Find selected data point
                DataPoint Datapoint = Chart.Series[0].Points[result.PointIndex];
                Datapoint.MarkerStyle = MarkerStyle.Square;
                Datapoint.MarkerColor = Color.Red;
                DateTime xValue = DateTime.FromOADate(Datapoint.XValue);
                string valueStr = _DataHistory.CcyDB.CcyToString(_DataHistory.TotalCcy, Datapoint.YValues[0]);
                Datapoint.ToolTip = $"{valueStr}{Environment.NewLine}{xValue:yyyy-MMM-dd}";

            }
        }

        private void comboBoxGraphTotalCcy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string valueCcy = comboBoxGraphTotalCcy.SelectedItem.ToString();
            if (valueCcy != _DataHistory.TotalCcy.CcyString)
            {
                _DataHistory.ModifyCcy(sender, new ModifyCcyEventArgs(valueCcy));
                Chart_Update();
                ShowTotal();
            }
        }

        #endregion

        #region DataGridViewStatics

        private void dataGridViewStaticsAsset_MouseDown(object sender, MouseEventArgs e)
        {
            dataGridViewStaticsAsset.CustomOnMouseDown(e);
        }

        private void dataGridViewStaticsAsset_MouseMove(object sender, MouseEventArgs e)
        {
            dataGridViewStaticsAsset.CustomOnMouseMove(e);
        }

        private void dataGridViewStaticsAsset_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point pt = dataGridViewStaticsAsset.PointToClient(new Point(e.X, e.Y));
            int rowIndex = dataGridViewStaticsAsset.GetRowIndex(pt);
            dataGridViewStaticsAsset.ShowLine(rowIndex);
        }

        private void dataGridViewStaticsAsset_DragDrop(object sender, DragEventArgs e)
        {
            dataGridViewStaticsAsset.OnDragDop(sender, e);
            //CcyDB.SetAssetOrder(dataGridViewStaticsAsset.AssetList);
        }

        private void dataGridViewStaticsCcy_MouseDown(object sender, MouseEventArgs e)
        {
            dataGridViewStaticsCcy.CustomOnMouseDown(e);
        }

        private void dataGridViewStaticsCcy_MouseMove(object sender, MouseEventArgs e)
        {
            dataGridViewStaticsCcy.CustomOnMouseMove(e);
        }

        private void dataGridViewStaticsCcy_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point pt = dataGridViewStaticsCcy.PointToClient(new Point(e.X, e.Y));
            int rowIndex = dataGridViewStaticsCcy.GetRowIndex(pt);
            dataGridViewStaticsCcy.ShowLine(rowIndex);
        }

        private void dataGridViewStaticsCcy_DragDrop(object sender, DragEventArgs e)
        {
            dataGridViewStaticsCcy.OnDragDop(sender, e);
            //CcyDB.SetCcyOrder(dataGridViewStaticsCcy.CcyList);
        }

        #endregion

        public void Summary_Update()
        {
            if (_CurrentDate.HasValue)
                dataGridViewSummary.Update(_DataHistory.GetData(_CurrentDate.Value));
        }

        private void dataGridViewSummary_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn sortCol = dataGridViewSummary.Columns[dataGridViewSummary.ColumnCount - 1];
            if (e.ColumnIndex == 0)
            {
                dataGridViewSummary.Update(Data);
                sortCol.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            else
            {
                ListSortDirection d = ListSortDirection.Descending;
                SortOrder o = SortOrder.Descending;
                if (sortCol.HeaderCell.SortGlyphDirection == SortOrder.Descending)
                {
                    d = ListSortDirection.Ascending;
                    o = SortOrder.Ascending;
                }
                dataGridViewSummary.Sort(sortCol, d);
                sortCol.HeaderCell.SortGlyphDirection = o;
            }
        }
    }
}
