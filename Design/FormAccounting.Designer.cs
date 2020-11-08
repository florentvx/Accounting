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
            this.labelTable = new System.Windows.Forms.Label();
            this.buttonTotal = new System.Windows.Forms.Button();
            this.dataGridViewAccounting = new Design.DataGridViewAccounting();
            this.TreeViewAccounting = new Design.TreeViewAccounting();
            this.dataGridViewMarket = new Design.DataGridViewMarket();
            this.labelMarket = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAccounting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarket)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1185, 28);
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
            this.buttonTotal.Name = "buttonTotal";
            this.buttonTotal.Size = new System.Drawing.Size(239, 28);
            this.buttonTotal.TabIndex = 4;
            this.buttonTotal.Text = "Show Total";
            this.buttonTotal.UseVisualStyleBackColor = true;
            this.buttonTotal.Click += new System.EventHandler(this.ButtonTotal_Click);
            // 
            // dataGridViewAccounting
            // 
            this.dataGridViewAccounting.AllowUserToAddRows = false;
            this.dataGridViewAccounting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAccounting.Location = new System.Drawing.Point(276, 65);
            this.dataGridViewAccounting.Name = "dataGridViewAccounting";
            this.dataGridViewAccounting.RowHeadersVisible = false;
            this.dataGridViewAccounting.RowTemplate.Height = 24;
            this.dataGridViewAccounting.Size = new System.Drawing.Size(641, 343);
            this.dataGridViewAccounting.TabIndex = 2;
            // 
            // TreeViewAccounting
            // 
            this.TreeViewAccounting.AllowDrop = true;
            this.TreeViewAccounting.Location = new System.Drawing.Point(12, 65);
            this.TreeViewAccounting.Name = "TreeViewAccounting";
            this.TreeViewAccounting.Size = new System.Drawing.Size(244, 343);
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
            // dataGridViewMarket
            // 
            this.dataGridViewMarket.AllowUserToAddRows = false;
            this.dataGridViewMarket.AllowUserToDeleteRows = false;
            this.dataGridViewMarket.AllowUserToResizeColumns = false;
            this.dataGridViewMarket.AllowUserToResizeRows = false;
            this.dataGridViewMarket.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMarket.Location = new System.Drawing.Point(933, 65);
            this.dataGridViewMarket.Name = "dataGridViewMarket";
            this.dataGridViewMarket.RowHeadersVisible = false;
            this.dataGridViewMarket.RowTemplate.Height = 24;
            this.dataGridViewMarket.Size = new System.Drawing.Size(240, 343);
            this.dataGridViewMarket.TabIndex = 5;
            // 
            // labelMarket
            // 
            this.labelMarket.AutoSize = true;
            this.labelMarket.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMarket.Location = new System.Drawing.Point(930, 34);
            this.labelMarket.Name = "labelMarket";
            this.labelMarket.Size = new System.Drawing.Size(54, 18);
            this.labelMarket.TabIndex = 6;
            this.labelMarket.Text = "Market";
            // 
            // FormAccounting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1185, 420);
            this.Controls.Add(this.labelMarket);
            this.Controls.Add(this.dataGridViewMarket);
            this.Controls.Add(this.buttonTotal);
            this.Controls.Add(this.labelTable);
            this.Controls.Add(this.dataGridViewAccounting);
            this.Controls.Add(this.TreeViewAccounting);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormAccounting";
            this.Text = "Accounting";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAccounting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarket)).EndInit();
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
        private System.Windows.Forms.Label labelMarket;
    }
}

