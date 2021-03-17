using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Finance;
using Core.Interfaces;
using Core.Statics;

namespace Design
{
    static class DataGridViewMarketStatics
    {
        public static string[] FXColumnNames = { "Ccy1", "Ccy2", "Value" };
        public static string[] AssetColumnNames = { "Asset", "Ccy", "Value" };
        public const int Column_Asset1 = 0;
        public const int Column_Asset2 = 1;
        public const int Column_Value = 2;
        public const int ColumnNumber = 3;
    }

    public class DataGridViewMarket : DataGridView
    {
        private bool _IsFX;
        public IMarket MarketShowed;
        public bool IsFX => _IsFX;

        public void SetUpTable()
        {
            ColumnCount = DataGridViewMarketStatics.ColumnNumber;
            for (int i = 0; i < ColumnCount; i++)
                Columns[i].Name = IsFX ? DataGridViewMarketStatics.FXColumnNames[i] : DataGridViewMarketStatics.AssetColumnNames[i];
            AllowUserToAddRows = false;
            foreach (DataGridViewColumn column in Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.Width = 50;
                if (column.Name == "Asset")
                    column.Width = 70;
                
            }
            RowHeadersVisible = false;
        }

        public DataGridViewMarket(bool isFX) : base()
        {
            _IsFX = isFX;
        }

        public void ShowMarket(IMarket mkt, CurrencyAssetStaticsDataBase ccyDB)
        {
            MarketShowed = mkt;
            Rows.Clear();
            foreach (var item in mkt.EnumerateData(ccyDB))
            {
                var titles = new object[] {
                    item.Item1.Item1.ToString(), item.Item1.Ccy2.ToString(), item.Item2
                };
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(this);
                dgvr.SetValues(titles);
                Rows.Add(dgvr);
            }
            ClearSelection();
        }

        protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewCell cell = Rows[e.RowIndex].Cells[e.ColumnIndex];
                switch (e.ColumnIndex)
                {
                    case DataGridViewMarketStatics.Column_Value:
                        cell.Selected = true;
                        BeginEdit(true);
                        break;
                    default:
                        cell.Selected = false;
                        break;
                }
            }
        }

        public event DataGridViewCellEventHandler ValueChanged;

        protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case DataGridViewMarketStatics.Column_Value:
                    DataGridViewCellEventHandler handler = ValueChanged;
                    handler?.Invoke(this, e);
                    break;
            }            
        }
    }

    public class DataGridViewFXMarket : DataGridViewMarket
    {
        public DataGridViewFXMarket() : base(true) { }
    }

    public class DataGridViewAssetMarket : DataGridViewMarket
    {
        public DataGridViewAssetMarket() : base(false) { }
    }
}