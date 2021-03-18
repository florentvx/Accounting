using Design.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Design.SubForms
{
    public partial class AboutApplicationForm : Form
    {
        private Bitmap MyImage;
        public void ShowMyImage()
        {
            // Sets up an image object to be displayed.
            if (MyImage != null)
            {
                MyImage.Dispose();
            }

            // Stretches the image to fit the pictureBox.
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            MyImage = new Bitmap(Resources.AccountingDonationBCHAddress);
            //pictureBox1.ClientSize = new Size(xSize, ySize);
            pictureBox1.Image = (Image)MyImage;
        }

        public AboutApplicationForm()
        {
            StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            ShowMyImage();
        }
    }
}
