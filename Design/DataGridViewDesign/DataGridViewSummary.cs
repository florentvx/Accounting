using Core;
using Core.Finance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Design.DataGridViewDesign
{
    public class DGVComparer : System.Collections.IComparer
    {
        Dictionary<string, double> _Data;
        ListSortDirection _Direction;

        int DirectionFactor { get { return _Direction == ListSortDirection.Ascending ? 1 : -1; } }

        public DGVComparer(Dictionary<string, double> data, ListSortDirection direction)
        {
            _Data = data;
            _Direction = direction;
        }

        public int Compare(object x, object y)
        {
            DataGridViewRow row1 = (DataGridViewRow)x;
            DataGridViewRow row2 = (DataGridViewRow)y;

            double value1 = _Data[(string)row1.Cells[0].Value];
            double value2 = _Data[(string)row2.Cells[0].Value];

            if (value1 > value2) { return DirectionFactor; }
            else if (value2 > value1) { return -DirectionFactor; }
            return 0;
        }
    }

    class DataGridViewSummary : DataGridView
    {
        private Dictionary<string, double> _Weights;

        public DataGridViewSummary() : base()
        {
        }

        public void Update(AccountingData ad)
        {
            _Weights = new Dictionary<string, double> { };
            SummaryReport sr = ad.GetSummary();
            Rows.Clear();
            ColumnCount = 4;
            Columns[0].HeaderText = "Ccy/Asset";
            Columns[1].HeaderText = "Total";
            Columns[2].HeaderText = "Total Converted";
            Columns[3].HeaderText = "Weight";

            for (int j = 0; j < ColumnCount; j++)
            {
                Columns[j].Width = 75;
                Columns[j].SortMode = DataGridViewColumnSortMode.Programmatic;
            }
            foreach (var item in ad.CciesAndAssets)
            {
                double amount = sr.Get(item);
                if (amount != 0)
                {
                    double convAmount = ad.GetQuote(item, ad.Ccy) * amount;
                    double weight = convAmount / ad.TotalValue;
                    _Weights[item.ToString()] = weight;

                    object[] values = { item.ToString(),
                                        ad.GetAmountToString(item, amount),
                                        ad.GetAmountToString(ad.Ccy, convAmount),
                                        $"{Math.Round(weight * 100, 2)} %" };
                    Rows.Add(values);
                }
            }
            ClearSelection();
        }

        public override void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction)
        {
            base.Sort(new DGVComparer(_Weights, direction));
        }
    }
}
