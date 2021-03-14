using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Statics;

namespace Design.SubForms
{
    public partial class AddCcyRefForm : Form
    {
        public string CcyName;
        public CurrencyStatics CcyStatics;
        private DateTime _Date = DateTime.Today;

        public DateTime GetDate() { return _Date.Date; }

        public AddCcyRefForm()
        {
            StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
            CcyStatics = new CurrencyStatics();
            Tuple<bool, string> test = CcyStatics.Load(TextBoxName.Text, TextSymbol.Text, TextThousandMarker.Text, TextDecimalMarker.Text, null);
            if (test.Item1)
            {
                _Date = DateTimePicker.Value;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(test.Item2);
                DialogResult = DialogResult.Abort;
            }
        }

        private void TextBoxName_TextChanged(object sender, EventArgs e)
        {
            CcyName = TextBoxName.Text.ToUpper();
            if (CcyName == "")
                CcyName = null;
        }
    }
}
