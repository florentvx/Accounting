namespace Design.SubForms
{
    partial class AddCcyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OKButton = new System.Windows.Forms.Button();
            this.TextBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TextSymbol = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TextThousandMarker = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TextDecimalMarker = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TextBoxValue = new System.Windows.Forms.TextBox();
            this.LabelValue = new System.Windows.Forms.Label();
            this.CheckBoxIsInversePair = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ComboBoxOthCcy = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(143, 266);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(91, 31);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // TextBoxName
            // 
            this.TextBoxName.Location = new System.Drawing.Point(135, 19);
            this.TextBoxName.Name = "TextBoxName";
            this.TextBoxName.Size = new System.Drawing.Size(90, 22);
            this.TextBoxName.TabIndex = 1;
            this.TextBoxName.TextChanged += new System.EventHandler(this.TextBoxName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Currency Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Currency Symbol";
            // 
            // TextSymbol
            // 
            this.TextSymbol.Location = new System.Drawing.Point(135, 46);
            this.TextSymbol.Name = "TextSymbol";
            this.TextSymbol.Size = new System.Drawing.Size(90, 22);
            this.TextSymbol.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Thousand Marker";
            // 
            // TextThousandMarker
            // 
            this.TextThousandMarker.Location = new System.Drawing.Point(135, 73);
            this.TextThousandMarker.Name = "TextThousandMarker";
            this.TextThousandMarker.Size = new System.Drawing.Size(18, 22);
            this.TextThousandMarker.TabIndex = 6;
            this.TextThousandMarker.Text = "3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Number Decimals";
            // 
            // TextDecimalMarker
            // 
            this.TextDecimalMarker.Location = new System.Drawing.Point(135, 101);
            this.TextDecimalMarker.Name = "TextDecimalMarker";
            this.TextDecimalMarker.Size = new System.Drawing.Size(18, 22);
            this.TextDecimalMarker.TabIndex = 8;
            this.TextDecimalMarker.Text = "2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.TextDecimalMarker);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.TextThousandMarker);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.TextSymbol);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.TextBoxName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 130);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Currency Definition";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(231, 104);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 17);
            this.label10.TabIndex = 12;
            this.label10.Text = "Ex: 2 → $ 1.00";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(231, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 17);
            this.label9.TabIndex = 11;
            this.label9.Text = "Ex: 3 → $ 1,000";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(231, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 17);
            this.label8.TabIndex = 10;
            this.label8.Text = "Ex: $";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(231, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "Ex: USD";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TextBoxValue);
            this.groupBox2.Controls.Add(this.LabelValue);
            this.groupBox2.Controls.Add(this.CheckBoxIsInversePair);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.ComboBoxOthCcy);
            this.groupBox2.Location = new System.Drawing.Point(12, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(345, 112);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Market Definition";
            // 
            // TextBoxValue
            // 
            this.TextBoxValue.Location = new System.Drawing.Point(135, 78);
            this.TextBoxValue.Name = "TextBoxValue";
            this.TextBoxValue.Size = new System.Drawing.Size(87, 22);
            this.TextBoxValue.TabIndex = 13;
            // 
            // LabelValue
            // 
            this.LabelValue.AutoSize = true;
            this.LabelValue.Location = new System.Drawing.Point(9, 81);
            this.LabelValue.Name = "LabelValue";
            this.LabelValue.Size = new System.Drawing.Size(60, 17);
            this.LabelValue.TabIndex = 12;
            this.LabelValue.Text = "???/???";
            // 
            // CheckBoxIsInversePair
            // 
            this.CheckBoxIsInversePair.AutoSize = true;
            this.CheckBoxIsInversePair.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBoxIsInversePair.Location = new System.Drawing.Point(6, 50);
            this.CheckBoxIsInversePair.Name = "CheckBoxIsInversePair";
            this.CheckBoxIsInversePair.Size = new System.Drawing.Size(147, 21);
            this.CheckBoxIsInversePair.TabIndex = 11;
            this.CheckBoxIsInversePair.Text = " Is Inverse Pair      ";
            this.CheckBoxIsInversePair.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CheckBoxIsInversePair.UseVisualStyleBackColor = true;
            this.CheckBoxIsInversePair.CheckedChanged += new System.EventHandler(this.CheckBoxIsInversePair_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Other Currency";
            // 
            // ComboBoxOthCcy
            // 
            this.ComboBoxOthCcy.FormattingEnabled = true;
            this.ComboBoxOthCcy.Location = new System.Drawing.Point(135, 20);
            this.ComboBoxOthCcy.Name = "ComboBoxOthCcy";
            this.ComboBoxOthCcy.Size = new System.Drawing.Size(90, 24);
            this.ComboBoxOthCcy.TabIndex = 0;
            this.ComboBoxOthCcy.SelectedIndexChanged += new System.EventHandler(this.ComboBoxOthCcy_SelectedIndexChanged);
            // 
            // AddCcyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 305);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCcyForm";
            this.Text = "Add Currency";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.TextBox TextBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextSymbol;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextThousandMarker;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextDecimalMarker;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox TextBoxValue;
        private System.Windows.Forms.Label LabelValue;
        private System.Windows.Forms.CheckBox CheckBoxIsInversePair;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ComboBoxOthCcy;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
    }
}