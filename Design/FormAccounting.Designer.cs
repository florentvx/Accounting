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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.marketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCurrencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAssetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelTable = new System.Windows.Forms.Label();
            this.buttonTotal = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridViewFXMarket = new Design.DataGridViewFXMarket();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridViewAssetMarket = new Design.DataGridViewAssetMarket();
            this.labelDate = new System.Windows.Forms.Label();
            this.ComboBoxDates = new System.Windows.Forms.ComboBox();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.DataPage = new System.Windows.Forms.TabPage();
            this.TreeViewAccounting = new Design.TreeViewAccounting();
            this.dataGridViewAccounting = new Design.DataGridViewAccounting();
            this.GraphPage = new System.Windows.Forms.TabPage();
            this.comboBoxGraphTotalCcy = new System.Windows.Forms.ComboBox();
            this.labelGraphTotalCcy = new System.Windows.Forms.Label();
            this.Chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.StaticsPage = new System.Windows.Forms.TabPage();
            this.dataGridViewStaticsAsset = new System.Windows.Forms.DataGridView();
            this.dataGridViewStaticsCcy = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFXMarket)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAssetMarket)).BeginInit();
            this.MainTabControl.SuspendLayout();
            this.DataPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAccounting)).BeginInit();
            this.GraphPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).BeginInit();
            this.StaticsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStaticsAsset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStaticsCcy)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.marketToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1223, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.LoadToolStripMenuItem_Click);
            // 
            // marketToolStripMenuItem
            // 
            this.marketToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCurrencyToolStripMenuItem,
            this.addAssetToolStripMenuItem,
            this.addDateToolStripMenuItem});
            this.marketToolStripMenuItem.Name = "marketToolStripMenuItem";
            this.marketToolStripMenuItem.Size = new System.Drawing.Size(69, 24);
            this.marketToolStripMenuItem.Text = "Market";
            // 
            // addCurrencyToolStripMenuItem
            // 
            this.addCurrencyToolStripMenuItem.Name = "addCurrencyToolStripMenuItem";
            this.addCurrencyToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.addCurrencyToolStripMenuItem.Text = "Add Currency";
            this.addCurrencyToolStripMenuItem.Click += new System.EventHandler(this.AddCurrencyToolStripMenuItem_Click);
            // 
            // addAssetToolStripMenuItem
            // 
            this.addAssetToolStripMenuItem.Name = "addAssetToolStripMenuItem";
            this.addAssetToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.addAssetToolStripMenuItem.Text = "Add Asset";
            this.addAssetToolStripMenuItem.Click += new System.EventHandler(this.AddAssetToolStripMenuItem_Click);
            // 
            // addDateToolStripMenuItem
            // 
            this.addDateToolStripMenuItem.Name = "addDateToolStripMenuItem";
            this.addDateToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.addDateToolStripMenuItem.Text = "Add Date";
            this.addDateToolStripMenuItem.Click += new System.EventHandler(this.AddDateToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applicationToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(64, 24);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // applicationToolStripMenuItem
            // 
            this.applicationToolStripMenuItem.Name = "applicationToolStripMenuItem";
            this.applicationToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.applicationToolStripMenuItem.Text = "Application";
            this.applicationToolStripMenuItem.Click += new System.EventHandler(this.ApplicationToolStripMenuItem_Click);
            // 
            // labelTable
            // 
            this.labelTable.AutoSize = true;
            this.labelTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTable.Location = new System.Drawing.Point(273, 12);
            this.labelTable.Name = "labelTable";
            this.labelTable.Size = new System.Drawing.Size(108, 18);
            this.labelTable.TabIndex = 3;
            this.labelTable.Text = "No File Loaded";
            // 
            // buttonTotal
            // 
            this.buttonTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTotal.Location = new System.Drawing.Point(674, 12);
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
            this.tabControl1.Location = new System.Drawing.Point(931, 12);
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
            this.dataGridViewFXMarket.RowHeadersWidth = 51;
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
            this.dataGridViewAssetMarket.RowHeadersWidth = 51;
            this.dataGridViewAssetMarket.Size = new System.Drawing.Size(235, 343);
            this.dataGridViewAssetMarket.TabIndex = 0;
            this.dataGridViewAssetMarket.ValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewAssetMarket_ValueChanged);
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDate.Location = new System.Drawing.Point(8, 13);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(39, 18);
            this.labelDate.TabIndex = 8;
            this.labelDate.Text = "Date";
            // 
            // ComboBoxDates
            // 
            this.ComboBoxDates.FormattingEnabled = true;
            this.ComboBoxDates.Location = new System.Drawing.Point(76, 12);
            this.ComboBoxDates.Name = "ComboBoxDates";
            this.ComboBoxDates.Size = new System.Drawing.Size(179, 24);
            this.ComboBoxDates.TabIndex = 9;
            this.ComboBoxDates.SelectedIndexChanged += new System.EventHandler(this.ComboBoxDates_SelectedIndexChanged);
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.DataPage);
            this.MainTabControl.Controls.Add(this.GraphPage);
            this.MainTabControl.Controls.Add(this.StaticsPage);
            this.MainTabControl.Location = new System.Drawing.Point(12, 31);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(1204, 439);
            this.MainTabControl.TabIndex = 10;
            this.MainTabControl.SelectedIndexChanged += new System.EventHandler(this.MainTabControl_SelectedIndexChanged);
            // 
            // DataPage
            // 
            this.DataPage.Controls.Add(this.labelTable);
            this.DataPage.Controls.Add(this.ComboBoxDates);
            this.DataPage.Controls.Add(this.TreeViewAccounting);
            this.DataPage.Controls.Add(this.labelDate);
            this.DataPage.Controls.Add(this.dataGridViewAccounting);
            this.DataPage.Controls.Add(this.tabControl1);
            this.DataPage.Controls.Add(this.buttonTotal);
            this.DataPage.Location = new System.Drawing.Point(4, 25);
            this.DataPage.Name = "DataPage";
            this.DataPage.Padding = new System.Windows.Forms.Padding(3);
            this.DataPage.Size = new System.Drawing.Size(1196, 410);
            this.DataPage.TabIndex = 0;
            this.DataPage.Text = "Data";
            this.DataPage.UseVisualStyleBackColor = true;
            // 
            // TreeViewAccounting
            // 
            this.TreeViewAccounting.AllowDrop = true;
            this.TreeViewAccounting.Location = new System.Drawing.Point(11, 49);
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
            // dataGridViewAccounting
            // 
            this.dataGridViewAccounting.AllowUserToAddRows = false;
            this.dataGridViewAccounting.AllowUserToDeleteRows = false;
            this.dataGridViewAccounting.AllowUserToResizeColumns = false;
            this.dataGridViewAccounting.AllowUserToResizeRows = false;
            this.dataGridViewAccounting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAccounting.Location = new System.Drawing.Point(272, 49);
            this.dataGridViewAccounting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewAccounting.Name = "dataGridViewAccounting";
            this.dataGridViewAccounting.RowHeadersVisible = false;
            this.dataGridViewAccounting.RowHeadersWidth = 51;
            this.dataGridViewAccounting.RowTemplate.Height = 24;
            this.dataGridViewAccounting.Size = new System.Drawing.Size(641, 343);
            this.dataGridViewAccounting.TabIndex = 2;
            this.dataGridViewAccounting.ShowTotalEventHandler += new System.EventHandler<System.EventArgs>(this.ShowTotal);
            // 
            // GraphPage
            // 
            this.GraphPage.Controls.Add(this.comboBoxGraphTotalCcy);
            this.GraphPage.Controls.Add(this.labelGraphTotalCcy);
            this.GraphPage.Controls.Add(this.Chart);
            this.GraphPage.Location = new System.Drawing.Point(4, 25);
            this.GraphPage.Name = "GraphPage";
            this.GraphPage.Padding = new System.Windows.Forms.Padding(3);
            this.GraphPage.Size = new System.Drawing.Size(1196, 410);
            this.GraphPage.TabIndex = 1;
            this.GraphPage.Text = "Graph";
            this.GraphPage.UseVisualStyleBackColor = true;
            // 
            // comboBoxGraphTotalCcy
            // 
            this.comboBoxGraphTotalCcy.FormattingEnabled = true;
            this.comboBoxGraphTotalCcy.Location = new System.Drawing.Point(6, 35);
            this.comboBoxGraphTotalCcy.Name = "comboBoxGraphTotalCcy";
            this.comboBoxGraphTotalCcy.Size = new System.Drawing.Size(118, 24);
            this.comboBoxGraphTotalCcy.TabIndex = 2;
            this.comboBoxGraphTotalCcy.SelectedIndexChanged += new System.EventHandler(this.comboBoxGraphTotalCcy_SelectedIndexChanged);
            // 
            // labelGraphTotalCcy
            // 
            this.labelGraphTotalCcy.AutoSize = true;
            this.labelGraphTotalCcy.Location = new System.Drawing.Point(27, 6);
            this.labelGraphTotalCcy.Name = "labelGraphTotalCcy";
            this.labelGraphTotalCcy.Size = new System.Drawing.Size(67, 17);
            this.labelGraphTotalCcy.TabIndex = 1;
            this.labelGraphTotalCcy.Text = "Total Ccy";
            // 
            // Chart
            // 
            this.Chart.BackColor = System.Drawing.SystemColors.Window;
            this.Chart.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Chart.BorderlineColor = System.Drawing.Color.Gray;
            chartArea1.Name = "ChartArea1";
            this.Chart.ChartAreas.Add(chartArea1);
            this.Chart.Location = new System.Drawing.Point(130, 8);
            this.Chart.Name = "Chart";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.Chart.Series.Add(series1);
            this.Chart.Size = new System.Drawing.Size(881, 398);
            this.Chart.TabIndex = 0;
            this.Chart.Text = "chart1";
            this.Chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Chart_MouseMove);
            // 
            // StaticsPage
            // 
            this.StaticsPage.Controls.Add(this.dataGridViewStaticsAsset);
            this.StaticsPage.Controls.Add(this.dataGridViewStaticsCcy);
            this.StaticsPage.Location = new System.Drawing.Point(4, 25);
            this.StaticsPage.Name = "StaticsPage";
            this.StaticsPage.Size = new System.Drawing.Size(1196, 410);
            this.StaticsPage.TabIndex = 2;
            this.StaticsPage.Text = "Statics";
            this.StaticsPage.UseVisualStyleBackColor = true;
            // 
            // dataGridViewStaticsAsset
            // 
            this.dataGridViewStaticsAsset.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStaticsAsset.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridViewStaticsAsset.Location = new System.Drawing.Point(549, 24);
            this.dataGridViewStaticsAsset.Name = "dataGridViewStaticsAsset";
            this.dataGridViewStaticsAsset.RowHeadersVisible = false;
            this.dataGridViewStaticsAsset.RowHeadersWidth = 51;
            this.dataGridViewStaticsAsset.RowTemplate.Height = 24;
            this.dataGridViewStaticsAsset.Size = new System.Drawing.Size(556, 382);
            this.dataGridViewStaticsAsset.TabIndex = 1;
            // 
            // dataGridViewStaticsCcy
            // 
            this.dataGridViewStaticsCcy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStaticsCcy.Location = new System.Drawing.Point(3, 24);
            this.dataGridViewStaticsCcy.Name = "dataGridViewStaticsCcy";
            this.dataGridViewStaticsCcy.RowHeadersVisible = false;
            this.dataGridViewStaticsCcy.RowHeadersWidth = 51;
            this.dataGridViewStaticsCcy.RowTemplate.Height = 24;
            this.dataGridViewStaticsCcy.Size = new System.Drawing.Size(521, 382);
            this.dataGridViewStaticsCcy.TabIndex = 0;
            // 
            // FormAccounting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 474);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAccounting";
            this.Text = "SortFin";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFXMarket)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAssetMarket)).EndInit();
            this.MainTabControl.ResumeLayout(false);
            this.DataPage.ResumeLayout(false);
            this.DataPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAccounting)).EndInit();
            this.GraphPage.ResumeLayout(false);
            this.GraphPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).EndInit();
            this.StaticsPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStaticsAsset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStaticsCcy)).EndInit();
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
        protected System.Windows.Forms.ComboBox ComboBoxDates;
        private System.Windows.Forms.ToolStripMenuItem marketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addCurrencyToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem addAssetToolStripMenuItem;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.TabPage tabPage2;
        protected DataGridViewAssetMarket dataGridViewAssetMarket;
        private System.Windows.Forms.ToolStripMenuItem addDateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applicationToolStripMenuItem;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage DataPage;
        private System.Windows.Forms.TabPage GraphPage;
        private System.Windows.Forms.DataVisualization.Charting.Chart Chart;
        private System.Windows.Forms.Label labelGraphTotalCcy;
        private System.Windows.Forms.ComboBox comboBoxGraphTotalCcy;
        private System.Windows.Forms.TabPage StaticsPage;
        private System.Windows.Forms.DataGridView dataGridViewStaticsCcy;
        private System.Windows.Forms.DataGridView dataGridViewStaticsAsset;
    }
}

