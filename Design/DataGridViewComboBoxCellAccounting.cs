using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Interfaces;

namespace Design
{
    public class DataGridViewComboBoxCellAccounting : DataGridViewComboBoxCell
    {
        public DataGridViewComboBoxCellAccounting(ICcyAsset ccy, IEnumerable<string> ccies): base()
        {
            foreach (string item in ccies)
            {
                Items.Add(item);
            }
            Value = ccy.ToString();
            FlatStyle = FlatStyle.Flat;
        }
    }
}
