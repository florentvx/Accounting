using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Finance;

namespace Design
{
    public class DataGridViewComboBoxCellAccounting : DataGridViewComboBoxCell
    {
        public DataGridViewComboBoxCellAccounting(Currency ccy): base()
        {
            var ccyList = CurrencyFunctions.GetCurrencyList();
            foreach (Currency item in ccyList)
            {
                Items.Add(item);
            }
            Value = ccy;
            FlatStyle = FlatStyle.Flat;
        }
    }
}
