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

namespace Design.SubForm
{
    public partial class AddAssetForm : Form
    {
        public string AssetName;
        public double AssetCcyPairQuote;

        public AssetCcyPair AssetCcyPair {
            get
            {
                //if (!CheckBoxIsInversePair.Checked)
                return new AssetCcyPair(new Asset(AssetName), new Currency(ComboBoxOthCcy.Text));
                //else
                //    return new AssetCcyPair(ComboBoxOthCcy.Text, AssetName);
            }
        }

        public void UpdateValueLabel()
        {
            //string ccyTemp = CcyName;
            //if (CcyName == null)
            //    ccyTemp = "???";
            //if (!CheckBoxIsInversePair.Checked)
            //    LabelValue.Text = ccyTemp + "/" + ComboBoxOthCcy.Text;
            //else
            //    LabelValue.Text = ComboBoxOthCcy.Text + "/" + ccyTemp;
            string assetTemp = AssetName;
            if (AssetName == null)
                assetTemp = "???";
            LabelValue.Text = assetTemp + "/" + ComboBoxOthCcy.Text;
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

        public AddAssetForm(IEnumerable<string> AvailableCcies)
        {
            InitializeComponent();
            InitComboBox(AvailableCcies);
            UpdateValueLabel();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
            //if (test.Item1)
            //{
            try { AssetCcyPairQuote = Convert.ToDouble(TextBoxValue.Text); }
            catch (Exception) { AssetCcyPairQuote = -1; }
            if (AssetCcyPairQuote > 0)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Abort;
            //}
            //else
            //{
            //    MessageBox.Show(test.Item2);
            //    DialogResult = DialogResult.Abort;
            //}
        }

        private void TextBoxName_TextChanged(object sender, EventArgs e)
        {
            AssetName = TextBoxName.Text.ToUpper();
            if (AssetName == "")
                AssetName = null;
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
