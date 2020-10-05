using System.Drawing;
using System.Windows.Forms;
using Core.Interfaces;
using Core;

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
                account.AccountName, account.Ccy, amount, account.Amount
            };

            SetValues(titles);
            if (!account.IsCalculatedAccount || isTotal)    
                Cells[DataGridViewAccountingStatics.Column_Currency] = 
                    new DataGridViewComboBoxCellAccounting(account.Ccy);
            if (account.IsCalculatedAccount)
                Cells[DataGridViewAccountingStatics.Column_Amount].ReadOnly = true;
            Cells[DataGridViewAccountingStatics.Column_ConvertedAmount].ReadOnly = true;

            if (isTotal)
            {
                for (int i = 0; i < Cells.Count; i++)
                {
                    Cells[i].Style.BackColor = Color.LightGray;
                    if (i != DataGridViewAccountingStatics.Column_Currency)
                        Cells[i].ReadOnly = true;
                }
            }
        }
    }
}
