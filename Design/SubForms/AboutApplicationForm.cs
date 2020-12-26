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
        public void ShowMyImage(string fileToDisplay, int xSize, int ySize)
        {
            // Sets up an image object to be displayed.
            if (MyImage != null)
            {
                MyImage.Dispose();
            }

            // Stretches the image to fit the pictureBox.
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            MyImage = new Bitmap(fileToDisplay);
            //pictureBox1.ClientSize = new Size(xSize, ySize);
            pictureBox1.Image = (Image)MyImage;
        }

        public AboutApplicationForm()
        {
            InitializeComponent();
            string filePath = Directory.GetCurrentDirectory();
            ShowMyImage(filePath + @"\..\..\..\Design\Images\AccountingDonationBCHAddress.png", 747, 626);
        }
    }
}
