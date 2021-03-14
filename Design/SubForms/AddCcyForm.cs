using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Finance;
using Core.Statics;

namespace Design.SubForms
{
    public partial class AddCcyForm : Form
    {
        public string CcyName;
        public CurrencyStatics CcyStatics;
        public double CcyPairQuote;

        public CurrencyPair CcyPair {
            get
            {
                if (!CheckBoxIsInversePair.Checked)
                    return new CurrencyPair(CcyName, ComboBoxOthCcy.Text);
                else
                    return new CurrencyPair(ComboBoxOthCcy.Text, CcyName);
            }
        }

        public void UpdateValueLabel()
        {
            string ccyTemp = CcyName;
            if (CcyName == null)
                ccyTemp = "???";
            if (!CheckBoxIsInversePair.Checked)
                LabelValue.Text = ccyTemp + "/" + ComboBoxOthCcy.Text;
            else
                LabelValue.Text = ComboBoxOthCcy.Text + "/" + ccyTemp;
        }

        public void InitComboBox(IEnumerable<string> AvailableCcies)
        {
            foreach (string item in AvailableCcies)
            {
                ComboBoxOthCcy.Items.Add(item);
            }
            ComboBoxOthCcy.Text = AvailableCcies.First();
            ComboBoxOthCcy.FlatStyle = FlatStyle.Flat;
        }

        public AddCcyForm(IEnumerable<string> AvailableCcies)
        {
            StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            InitComboBox(AvailableCcies);
            UpdateValueLabel();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
            CcyStatics = new CurrencyStatics();
            Tuple<bool, string> test = CcyStatics.Load(TextBoxName.Text, TextSymbol.Text, TextThousandMarker.Text, TextDecimalMarker.Text, CcyPair);
            if (test.Item1)
            {
                try { CcyPairQuote = Convert.ToDouble(TextBoxValue.Text); }
                catch (Exception){ CcyPairQuote = -1; }
                if (CcyPairQuote > 0)
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
            UpdateValueLabel();
        }

        private void CheckBoxIsInversePair_CheckedChanged(object sender, EventArgs e)
        {
            UpdateValueLabel();
        }

        private void ComboBoxOthCcy_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateValueLabel();
        }
    }
}
