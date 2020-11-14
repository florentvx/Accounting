using Design;

namespace Accounting
{
    partial class FormAccounting
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.marketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCurrencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelTable = new System.Windows.Forms.Label();
            this.buttonTotal = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewMarket = new Design.DataGridViewMarket();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewAccounting = new Design.DataGridViewAccounting();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TreeViewAccounting = new Design.TreeViewAccounting();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAccounting)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.marketToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(904, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // marketToolStripMenuItem
            // 
            this.marketToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCurrencyToolStripMenuItem});
            this.marketToolStripMenuItem.Name = "marketToolStripMenuItem";
            this.marketToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.marketToolStripMenuItem.Text = "Market";
            // 
            // addCurrencyToolStripMenuItem
            // 
            this.addCurrencyToolStripMenuItem.Name = "addCurrencyToolStripMenuItem";
            this.addCurrencyToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.addCurrencyToolStripMenuItem.Text = "Add Currency";
            this.addCurrencyToolStripMenuItem.Click += new System.EventHandler(this.AddCurrencyToolStripMenuItem_Click);
            // 
            // labelTable
            // 
            this.labelTable.AutoSize = true;
            this.labelTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTable.Location = new System.Drawing.Point(208, 28);
            this.labelTable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTable.Name = "labelTable";
            this.labelTable.Size = new System.Drawing.Size(69, 15);
            this.labelTable.TabIndex = 3;
            this.labelTable.Text = "LabelTable";
            // 
            // buttonTotal
            // 
            this.buttonTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTotal.Location = new System.Drawing.Point(12, 25);
            this.buttonTotal.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTotal.Name = "buttonTotal";
            this.buttonTotal.Size = new System.Drawing.Size(179, 23);
            this.buttonTotal.TabIndex = 4;
            this.buttonTotal.Text = "Show Total";
            this.buttonTotal.UseVisualStyleBackColor = true;
            this.buttonTotal.Click += new System.EventHandler(this.ButtonTotal_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(701, 28);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(192, 318);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(184, 292);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Currencies";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridViewMarket);
            this.panel1.Location = new System.Drawing.Point(-4, 5);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(188, 285);
            this.panel1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(184, 292);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Assets";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 50;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 50;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewMarket
            // 
            this.dataGridViewMarket.AllowUserToAddRows = false;
            this.dataGridViewMarket.AllowUserToDeleteRows = false;
            this.dataGridViewMarket.AllowUserToResizeColumns = false;
            this.dataGridViewMarket.AllowUserToResizeRows = false;
            this.dataGridViewMarket.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMarket.Location = new System.Drawing.Point(8, 3);
            this.dataGridViewMarket.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewMarket.Name = "dataGridViewMarket";
            this.dataGridViewMarket.RowHeadersVisible = false;
            this.dataGridViewMarket.RowTemplate.Height = 24;
            this.dataGridViewMarket.Size = new System.Drawing.Size(176, 279);
            this.dataGridViewMarket.TabIndex = 5;
            this.dataGridViewMarket.ValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewMarket_ValueChanged);
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn8.Width = 50;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn9.Width = 50;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn10.Width = 50;
            // 
            // dataGridViewAccounting
            // 
            this.dataGridViewAccounting.AllowUserToAddRows = false;
            this.dataGridViewAccounting.AllowUserToDeleteRows = false;
            this.dataGridViewAccounting.AllowUserToResizeColumns = false;
            this.dataGridViewAccounting.AllowUserToResizeRows = false;
            this.dataGridViewAccounting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAccounting.Location = new System.Drawing.Point(207, 58);
            this.dataGridViewAccounting.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewAccounting.Name = "dataGridViewAccounting";
            this.dataGridViewAccounting.RowHeadersVisible = false;
            this.dataGridViewAccounting.RowTemplate.Height = 24;
            this.dataGridViewAccounting.Size = new System.Drawing.Size(481, 279);
            this.dataGridViewAccounting.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TreeViewAccounting
            // 
            this.TreeViewAccounting.AllowDrop = true;
            this.TreeViewAccounting.Location = new System.Drawing.Point(11, 58);
            this.TreeViewAccounting.Margin = new System.Windows.Forms.Padding(2);
            this.TreeViewAccounting.Name = "TreeViewAccounting";
            this.TreeViewAccounting.Size = new System.Drawing.Size(184, 279);
            this.TreeViewAccounting.TabIndex = 1;
            this.TreeViewAccounting.NodeAdded += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_NodeAddition);
            this.TreeViewAccounting.NodeDeleted += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_NodeDeletion);
            this.TreeViewAccounting.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAccounting_AfterCollapse);
            this.TreeViewAccounting.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAccounting_AfterExpand);
            this.TreeViewAccounting.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeViewAccounting_ItemDrag);
            this.TreeViewAccounting.DragDrop += new System.Windows.Forms.DragEventHandler(this.TreeViewAccounting_DragDrop);
            this.TreeViewAccounting.DragEnter += new System.Windows.Forms.DragEventHandler(this.TreeViewAccounting_DragEnter);
            this.TreeViewAccounting.DragOver += new System.Windows.Forms.DragEventHandler(this.TreeViewAccounting_DragOver);
            // 
            // FormAccounting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 350);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonTotal);
            this.Controls.Add(this.labelTable);
            this.Controls.Add(this.dataGridViewAccounting);
            this.Controls.Add(this.TreeViewAccounting);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormAccounting";
            this.Text = "Accounting";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAccounting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.MenuStrip menuStrip1;
        protected TreeViewAccounting TreeViewAccounting;
        protected DataGridViewAccounting dataGridViewAccounting;
        protected System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        protected System.Windows.Forms.Label labelTable;
        private System.Windows.Forms.Button buttonTotal;
        protected DataGridViewMarket dataGridViewMarket;
        private System.Windows.Forms.ToolStripMenuItem marketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addCurrencyToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
    }
}

