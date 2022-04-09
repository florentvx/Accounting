using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Design.SubForms
{
    public partial class AddDateForm : Form
    {
        public DateTime _Date = DateTime.Today;

        public long GetDate() { return _Date.Date.Ticks; }

        public AddDateForm()
        {
            StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            _Date = DateTimePicker.Value;
            DialogResult = DialogResult.OK;
        }
    }
}
