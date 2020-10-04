using System.Drawing;
using System.Windows.Forms;
using Core.Interfaces;

namespace Design
{
    class DataGridViewRowAccounting : DataGridViewRow
    {
        public DataGridViewRowAccounting(   DataGridViewAccounting table, 
                                            IAccount account, 
                                            bool isTotal = false)
        {
            CreateCells(table);
            string amount = account.Amount.ToString();
            if (isTotal)
                amount = "";
            var titles = new object[] {
                account.AccountName, account.Ccy.ToString(), amount, account.Amount
            };
            SetValues(titles);
            if (isTotal)
            {
                ReadOnly = isTotal;
                for (int i = 0; i < Cells.Count; i++)
                {
                    Cells[i].Style.BackColor = Color.LightGray;
                }
            }
        }
    }
}
