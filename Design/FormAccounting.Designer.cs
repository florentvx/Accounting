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
            this.dataGridViewAccounting = new Design.DataGridViewAccounting();
            this.TreeViewAccounting = new Design.TreeViewAccounting();
            this.dataGridViewFXMarket = new Design.DataGridViewFXMarket();
            this.dataGridViewAssetMarket = new Design.DataGridViewAssetMarket();
            this.addAssetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFXMarket)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAssetMarket)).BeginInit();
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
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1205, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(114, 26);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // marketToolStripMenuItem
            // 
            this.marketToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCurrencyToolStripMenuItem,
            this.addAssetToolStripMenuItem});
            this.marketToolStripMenuItem.Name = "marketToolStripMenuItem";
            this.marketToolStripMenuItem.Size = new System.Drawing.Size(67, 24);
            this.marketToolStripMenuItem.Text = "Market";
            // 
            // addCurrencyToolStripMenuItem
            // 
            this.addCurrencyToolStripMenuItem.Name = "addCurrencyToolStripMenuItem";
            this.addCurrencyToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.addCurrencyToolStripMenuItem.Text = "Add Currency";
            this.addCurrencyToolStripMenuItem.Click += new System.EventHandler(this.AddCurrencyToolStripMenuItem_Click);
            // 
            // labelTable
            // 
            this.labelTable.AutoSize = true;
            this.labelTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTable.Location = new System.Drawing.Point(277, 34);
            this.labelTable.Name = "labelTable";
            this.labelTable.Size = new System.Drawing.Size(79, 18);
            this.labelTable.TabIndex = 3;
            this.labelTable.Text = "LabelTable";
            // 
            // buttonTotal
            // 
            this.buttonTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTotal.Location = new System.Drawing.Point(16, 31);
            this.buttonTotal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTotal.Name = "buttonTotal";
            this.buttonTotal.Size = new System.Drawing.Size(239, 28);
            this.buttonTotal.TabIndex = 4;
            this.buttonTotal.Text = "Show Total";
            this.buttonTotal.UseVisualStyleBackColor = true;
            this.buttonTotal.Click += new System.EventHandler(this.ButtonTotal_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(935, 34);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(256, 391);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(248, 362);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Currencies";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridViewFXMarket);
            this.panel1.Location = new System.Drawing.Point(-5, 6);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(251, 351);
            this.panel1.TabIndex = 0;
            // 
            // dataGridViewFXMarket
            // 
            this.dataGridViewFXMarket.AllowUserToAddRows = false;
            this.dataGridViewFXMarket.AllowUserToDeleteRows = false;
            this.dataGridViewFXMarket.AllowUserToResizeColumns = false;
            this.dataGridViewFXMarket.AllowUserToResizeRows = false;
            this.dataGridViewFXMarket.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFXMarket.Location = new System.Drawing.Point(11, 4);
            this.dataGridViewFXMarket.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewFXMarket.Name = "dataGridViewFXMarket";
            this.dataGridViewFXMarket.RowHeadersVisible = false;
            this.dataGridViewFXMarket.RowTemplate.Height = 24;
            this.dataGridViewFXMarket.Size = new System.Drawing.Size(235, 343);
            this.dataGridViewFXMarket.TabIndex = 5;
            this.dataGridViewFXMarket.ValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewFXMarket_ValueChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridViewAssetMarket);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(248, 362);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Assets";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridViewAssetMarket
            // 
            this.dataGridViewAssetMarket.AllowUserToAddRows = false;
            this.dataGridViewAssetMarket.AllowUserToDeleteRows = false;
            this.dataGridViewAssetMarket.AllowUserToResizeColumns = false;
            this.dataGridViewAssetMarket.AllowUserToResizeRows = false;
            this.dataGridViewAssetMarket.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAssetMarket.Location = new System.Drawing.Point(4, 10);
            this.dataGridViewAssetMarket.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewAssetMarket.Name = "dataGridViewAssetMarket";
            this.dataGridViewAssetMarket.RowHeadersVisible = false;
            this.dataGridViewAssetMarket.Size = new System.Drawing.Size(235, 343);
            this.dataGridViewAssetMarket.TabIndex = 0;
            this.dataGridViewAssetMarket.ValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewAssetMarket_ValueChanged);
            // 
            // dataGridViewAccounting
            // 
            this.dataGridViewAccounting.AllowUserToAddRows = false;
            this.dataGridViewAccounting.AllowUserToDeleteRows = false;
            this.dataGridViewAccounting.AllowUserToResizeColumns = false;
            this.dataGridViewAccounting.AllowUserToResizeRows = false;
            this.dataGridViewAccounting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAccounting.Location = new System.Drawing.Point(276, 71);
            this.dataGridViewAccounting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewAccounting.Name = "dataGridViewAccounting";
            this.dataGridViewAccounting.RowHeadersVisible = false;
            this.dataGridViewAccounting.RowTemplate.Height = 24;
            this.dataGridViewAccounting.Size = new System.Drawing.Size(641, 343);
            this.dataGridViewAccounting.TabIndex = 2;
            // 
            // TreeViewAccounting
            // 
            this.TreeViewAccounting.AllowDrop = true;
            this.TreeViewAccounting.Location = new System.Drawing.Point(15, 71);
            this.TreeViewAccounting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TreeViewAccounting.Name = "TreeViewAccounting";
            this.TreeViewAccounting.Size = new System.Drawing.Size(244, 342);
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
            // addAssetToolStripMenuItem
            // 
            this.addAssetToolStripMenuItem.Name = "addAssetToolStripMenuItem";
            this.addAssetToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.addAssetToolStripMenuItem.Text = "Add Asset";
            this.addAssetToolStripMenuItem.Click += new System.EventHandler(this.AddAssetToolStripMenuItem_Click);
            // 
            // FormAccounting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1205, 431);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonTotal);
            this.Controls.Add(this.labelTable);
            this.Controls.Add(this.dataGridViewAccounting);
            this.Controls.Add(this.TreeViewAccounting);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormAccounting";
            this.Text = "Accounting";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFXMarket)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAssetMarket)).EndInit();
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
        protected System.Windows.Forms.Button buttonTotal;
        protected DataGridViewFXMarket dataGridViewFXMarket;
        protected DataGridViewAssetMarket dataGridViewAssetMarket;
        private System.Windows.Forms.ToolStripMenuItem marketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addCurrencyToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem addAssetToolStripMenuItem;
    }
}

