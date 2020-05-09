namespace Webshot
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtBaseUrl = new System.Windows.Forms.TextBox();
            this.pnlCrawl = new System.Windows.Forms.GroupBox();
            this.btnStartCrawl = new System.Windows.Forms.Button();
            this.cbCrawlExternalLinks = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlSelectedPages = new System.Windows.Forms.GroupBox();
            this.txtSelectedPages = new System.Windows.Forms.TextBox();
            this.btnStartScreenshots = new System.Windows.Forms.Button();
            this.pnlOptions = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnViewResults = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnChooseOutput = new System.Windows.Forms.Button();
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.numDesktopWidth = new System.Windows.Forms.NumericUpDown();
            this.numTabletWidth = new System.Windows.Forms.NumericUpDown();
            this.numMobileWidth = new System.Windows.Forms.NumericUpDown();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveSettingsDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openManifestDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.btnCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlCrawl.SuspendLayout();
            this.pnlSelectedPages.SuspendLayout();
            this.pnlOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDesktopWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTabletWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMobileWidth)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBaseUrl
            // 
            this.txtBaseUrl.Location = new System.Drawing.Point(112, 43);
            this.txtBaseUrl.Name = "txtBaseUrl";
            this.txtBaseUrl.Size = new System.Drawing.Size(631, 29);
            this.txtBaseUrl.TabIndex = 0;
            // 
            // pnlCrawl
            // 
            this.pnlCrawl.Controls.Add(this.btnStartCrawl);
            this.pnlCrawl.Controls.Add(this.cbCrawlExternalLinks);
            this.pnlCrawl.Controls.Add(this.label1);
            this.pnlCrawl.Controls.Add(this.txtBaseUrl);
            this.pnlCrawl.Location = new System.Drawing.Point(21, 12);
            this.pnlCrawl.Name = "pnlCrawl";
            this.pnlCrawl.Size = new System.Drawing.Size(1066, 244);
            this.pnlCrawl.TabIndex = 1;
            this.pnlCrawl.TabStop = false;
            this.pnlCrawl.Text = "Crawl Pages";
            // 
            // btnStartCrawl
            // 
            this.btnStartCrawl.Location = new System.Drawing.Point(11, 124);
            this.btnStartCrawl.Name = "btnStartCrawl";
            this.btnStartCrawl.Size = new System.Drawing.Size(196, 52);
            this.btnStartCrawl.TabIndex = 4;
            this.btnStartCrawl.Text = "Start";
            this.btnStartCrawl.UseVisualStyleBackColor = true;
            this.btnStartCrawl.Click += new System.EventHandler(this.BtnStartCrawl_Click);
            // 
            // cbCrawlExternalLinks
            // 
            this.cbCrawlExternalLinks.AutoSize = true;
            this.cbCrawlExternalLinks.Location = new System.Drawing.Point(14, 89);
            this.cbCrawlExternalLinks.Name = "cbCrawlExternalLinks";
            this.cbCrawlExternalLinks.Size = new System.Drawing.Size(221, 29);
            this.cbCrawlExternalLinks.TabIndex = 3;
            this.cbCrawlExternalLinks.Text = "Follow External Links";
            this.cbCrawlExternalLinks.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Base URL";
            // 
            // pnlSelectedPages
            // 
            this.pnlSelectedPages.Controls.Add(this.txtSelectedPages);
            this.pnlSelectedPages.Controls.Add(this.btnStartScreenshots);
            this.pnlSelectedPages.Location = new System.Drawing.Point(21, 526);
            this.pnlSelectedPages.Name = "pnlSelectedPages";
            this.pnlSelectedPages.Size = new System.Drawing.Size(1066, 428);
            this.pnlSelectedPages.TabIndex = 2;
            this.pnlSelectedPages.TabStop = false;
            this.pnlSelectedPages.Text = "Selected Pages";
            // 
            // txtSelectedPages
            // 
            this.txtSelectedPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSelectedPages.Location = new System.Drawing.Point(3, 25);
            this.txtSelectedPages.Multiline = true;
            this.txtSelectedPages.Name = "txtSelectedPages";
            this.txtSelectedPages.Size = new System.Drawing.Size(1060, 329);
            this.txtSelectedPages.TabIndex = 0;
            // 
            // btnStartScreenshots
            // 
            this.btnStartScreenshots.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnStartScreenshots.Location = new System.Drawing.Point(3, 354);
            this.btnStartScreenshots.Name = "btnStartScreenshots";
            this.btnStartScreenshots.Size = new System.Drawing.Size(1060, 71);
            this.btnStartScreenshots.TabIndex = 1;
            this.btnStartScreenshots.Text = "Start Screenshots";
            this.btnStartScreenshots.UseVisualStyleBackColor = true;
            this.btnStartScreenshots.Click += new System.EventHandler(this.BtnStartScreenshots_Click);
            // 
            // pnlOptions
            // 
            this.pnlOptions.Controls.Add(this.label5);
            this.pnlOptions.Controls.Add(this.label4);
            this.pnlOptions.Controls.Add(this.label3);
            this.pnlOptions.Controls.Add(this.btnViewResults);
            this.pnlOptions.Controls.Add(this.label2);
            this.pnlOptions.Controls.Add(this.btnChooseOutput);
            this.pnlOptions.Controls.Add(this.txtOutputDir);
            this.pnlOptions.Controls.Add(this.btnExport);
            this.pnlOptions.Controls.Add(this.btnImport);
            this.pnlOptions.Controls.Add(this.numDesktopWidth);
            this.pnlOptions.Controls.Add(this.numTabletWidth);
            this.pnlOptions.Controls.Add(this.numMobileWidth);
            this.pnlOptions.Location = new System.Drawing.Point(21, 271);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(1060, 249);
            this.pnlOptions.TabIndex = 3;
            this.pnlOptions.TabStop = false;
            this.pnlOptions.Text = "Options";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 25);
            this.label5.TabIndex = 14;
            this.label5.Text = "Desktop";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 25);
            this.label4.TabIndex = 13;
            this.label4.Text = "Tablet";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "Mobile";
            // 
            // btnViewResults
            // 
            this.btnViewResults.Location = new System.Drawing.Point(586, 46);
            this.btnViewResults.Name = "btnViewResults";
            this.btnViewResults.Size = new System.Drawing.Size(295, 52);
            this.btnViewResults.TabIndex = 11;
            this.btnViewResults.Text = "View Existing Results";
            this.btnViewResults.UseVisualStyleBackColor = true;
            this.btnViewResults.Click += new System.EventHandler(this.BtnViewResults_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 25);
            this.label2.TabIndex = 10;
            this.label2.Text = "Output Dir";
            // 
            // btnChooseOutput
            // 
            this.btnChooseOutput.Location = new System.Drawing.Point(689, 186);
            this.btnChooseOutput.Name = "btnChooseOutput";
            this.btnChooseOutput.Size = new System.Drawing.Size(116, 42);
            this.btnChooseOutput.TabIndex = 9;
            this.btnChooseOutput.Text = "Browse...";
            this.btnChooseOutput.UseVisualStyleBackColor = true;
            this.btnChooseOutput.Click += new System.EventHandler(this.BtnChooseOutput_Click);
            // 
            // txtOutputDir
            // 
            this.txtOutputDir.Location = new System.Drawing.Point(117, 192);
            this.txtOutputDir.Name = "txtOutputDir";
            this.txtOutputDir.Size = new System.Drawing.Size(566, 29);
            this.txtOutputDir.TabIndex = 8;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(260, 112);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(191, 52);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export Settings";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(260, 46);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(191, 52);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import Settings";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // numDesktopWidth
            // 
            this.numDesktopWidth.Location = new System.Drawing.Point(124, 134);
            this.numDesktopWidth.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numDesktopWidth.Name = "numDesktopWidth";
            this.numDesktopWidth.Size = new System.Drawing.Size(120, 29);
            this.numDesktopWidth.TabIndex = 5;
            this.numDesktopWidth.Tag = "Desktop";
            this.numDesktopWidth.ValueChanged += new System.EventHandler(this.DeviceWidth_Changed);
            // 
            // numTabletWidth
            // 
            this.numTabletWidth.Location = new System.Drawing.Point(124, 90);
            this.numTabletWidth.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numTabletWidth.Name = "numTabletWidth";
            this.numTabletWidth.Size = new System.Drawing.Size(120, 29);
            this.numTabletWidth.TabIndex = 3;
            this.numTabletWidth.Tag = "Tablet";
            this.numTabletWidth.ValueChanged += new System.EventHandler(this.DeviceWidth_Changed);
            // 
            // numMobileWidth
            // 
            this.numMobileWidth.Location = new System.Drawing.Point(124, 46);
            this.numMobileWidth.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numMobileWidth.Name = "numMobileWidth";
            this.numMobileWidth.Size = new System.Drawing.Size(120, 29);
            this.numMobileWidth.TabIndex = 1;
            this.numMobileWidth.Tag = "Mobile";
            this.numMobileWidth.ValueChanged += new System.EventHandler(this.DeviceWidth_Changed);
            // 
            // saveSettingsDialog
            // 
            this.saveSettingsDialog.FileName = "settings.json";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "json";
            this.openFileDialog1.FileName = "settings";
            this.openFileDialog1.Filter = "JSON Files (*.json)|*.json";
            // 
            // openManifestDialog
            // 
            this.openManifestDialog.DefaultExt = "json";
            this.openManifestDialog.FileName = "manifest.json";
            this.openManifestDialog.Filter = "Screenshot Manifest|manifest.json";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar,
            this.lblStatus,
            this.toolStripSplitButton1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1018);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1583, 38);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 28);
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 29);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCancel});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(106, 34);
            this.toolStripSplitButton1.Text = "Actions";
            // 
            // btnCancel
            // 
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(193, 40);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1583, 1056);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlOptions);
            this.Controls.Add(this.pnlSelectedPages);
            this.Controls.Add(this.pnlCrawl);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlCrawl.ResumeLayout(false);
            this.pnlCrawl.PerformLayout();
            this.pnlSelectedPages.ResumeLayout(false);
            this.pnlSelectedPages.PerformLayout();
            this.pnlOptions.ResumeLayout(false);
            this.pnlOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDesktopWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTabletWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMobileWidth)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBaseUrl;
        private System.Windows.Forms.GroupBox pnlCrawl;
        private System.Windows.Forms.CheckBox cbCrawlExternalLinks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStartCrawl;
        private System.Windows.Forms.GroupBox pnlSelectedPages;
        private System.Windows.Forms.TextBox txtSelectedPages;
        private System.Windows.Forms.GroupBox pnlOptions;
        private System.Windows.Forms.NumericUpDown numDesktopWidth;
        private System.Windows.Forms.NumericUpDown numTabletWidth;
        private System.Windows.Forms.NumericUpDown numMobileWidth;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChooseOutput;
        private System.Windows.Forms.TextBox txtOutputDir;
        private System.Windows.Forms.SaveFileDialog saveSettingsDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnStartScreenshots;
        private System.Windows.Forms.Button btnViewResults;
        private System.Windows.Forms.OpenFileDialog openManifestDialog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem btnCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}

