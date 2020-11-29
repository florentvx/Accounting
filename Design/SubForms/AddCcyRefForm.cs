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

namespace Design.SubForm
{
    public partial class AddCcyRefForm : Form
    {
        public string CcyName;
        public CurrencyStatics CcyStatics;

        public AddCcyRefForm()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
            CcyStatics = new CurrencyStatics();
            Tuple<bool, string> test = CcyStatics.Load(TextSymbol.Text, TextThousandMarker.Text, TextDecimalMarker.Text);
            if (test.Item1)
            {
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
