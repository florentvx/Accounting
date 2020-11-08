using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Finance;

namespace Design
{
    static class DataGridViewMarketStatics
    {
        public static string[] ColumnNames = { "Asset1", "Asset2", "Value"};
        public const int Column_Asset1 = 0;
        public const int Column_Asset2 = 1;
        public const int Column_Value = 2;
        public const int ColumnNumber = 3;
    }

    public class DataGridViewMarket : DataGridView
    {
        private void SetUpTable()
        {
            ColumnCount = DataGridViewMarketStatics.ColumnNumber;
            for (int i = 0; i < ColumnCount; i++)
                Columns[i].Name = DataGridViewMarketStatics.ColumnNames[i];
            AllowUserToAddRows = false;
            foreach (DataGridViewColumn column in Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.Width = 50;
            }
            RowHeadersVisible = false;
        }

        public DataGridViewMarket() : base()
        {
            SetUpTable();
        }

        public void ShowMarket(Market mkt)
        {
            Rows.Clear();
            foreach (var item in mkt.EnumerateData())
            {
                var titles = new object[] {
                    item.Item1.Ccy1.ToString(), item.Item1.Ccy2.ToString(), item.Item2
                };
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(this);
                dgvr.SetValues(titles);
                Rows.Add(dgvr);
            }
            Rows[Rows.Count - 1].Selected = true;
        }

        protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
        {
            DataGridViewCell cell = Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.Selected = false;
        }
    }
}
