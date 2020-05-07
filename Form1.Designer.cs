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
            this.label2 = new System.Windows.Forms.Label();
            this.btnChooseOutput = new System.Windows.Forms.Button();
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.numDesktop = new System.Windows.Forms.NumericUpDown();
            this.cbDesktop = new System.Windows.Forms.CheckBox();
            this.numTablet = new System.Windows.Forms.NumericUpDown();
            this.cbTablet = new System.Windows.Forms.CheckBox();
            this.numMobileWidth = new System.Windows.Forms.NumericUpDown();
            this.cbMobile = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnViewResults = new System.Windows.Forms.Button();
            this.openManifestDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.btnCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlCrawl.SuspendLayout();
            this.pnlSelectedPages.SuspendLayout();
            this.pnlOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDesktop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTablet)).BeginInit();
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
            this.pnlCrawl.Text = "Crawl Sites";
            // 
            // btnStartCrawl
            // 
            this.btnStartCrawl.Location = new System.Drawing.Point(11, 124);
            this.btnStartCrawl.Name = "btnStartCrawl";
            this.btnStartCrawl.Size = new System.Drawing.Size(196, 52);
            this.btnStartCrawl.TabIndex = 4;
            this.btnStartCrawl.Text = "Start";
            this.btnStartCrawl.UseVisualStyleBackColor = true;
            this.btnStartCrawl.Click += new System.EventHandler(this.btnStartCrawl_Click);
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
            this.btnStartScreenshots.Click += new System.EventHandler(this.btnStartScreenshots_Click);
            // 
            // pnlOptions
            // 
            this.pnlOptions.Controls.Add(this.btnViewResults);
            this.pnlOptions.Controls.Add(this.label2);
            this.pnlOptions.Controls.Add(this.btnChooseOutput);
            this.pnlOptions.Controls.Add(this.txtOutputDir);
            this.pnlOptions.Controls.Add(this.btnExport);
            this.pnlOptions.Controls.Add(this.btnImport);
            this.pnlOptions.Controls.Add(this.numDesktop);
            this.pnlOptions.Controls.Add(this.cbDesktop);
            this.pnlOptions.Controls.Add(this.numTablet);
            this.pnlOptions.Controls.Add(this.cbTablet);
            this.pnlOptions.Controls.Add(this.numMobileWidth);
            this.pnlOptions.Controls.Add(this.cbMobile);
            this.pnlOptions.Location = new System.Drawing.Point(21, 271);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(1060, 249);
            this.pnlOptions.TabIndex = 3;
            this.pnlOptions.TabStop = false;
            this.pnlOptions.Text = "Options";
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
            this.btnChooseOutput.Click += new System.EventHandler(this.btnChooseOutput_Click);
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
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(260, 46);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(191, 52);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import Settings";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // numDesktop
            // 
            this.numDesktop.Location = new System.Drawing.Point(124, 134);
            this.numDesktop.Name = "numDesktop";
            this.numDesktop.Size = new System.Drawing.Size(120, 29);
            this.numDesktop.TabIndex = 5;
            // 
            // cbDesktop
            // 
            this.cbDesktop.AutoSize = true;
            this.cbDesktop.Checked = true;
            this.cbDesktop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDesktop.Location = new System.Drawing.Point(8, 135);
            this.cbDesktop.Name = "cbDesktop";
            this.cbDesktop.Size = new System.Drawing.Size(110, 29);
            this.cbDesktop.TabIndex = 4;
            this.cbDesktop.Text = "Desktop";
            this.cbDesktop.UseVisualStyleBackColor = true;
            // 
            // numTablet
            // 
            this.numTablet.Location = new System.Drawing.Point(124, 90);
            this.numTablet.Name = "numTablet";
            this.numTablet.Size = new System.Drawing.Size(120, 29);
            this.numTablet.TabIndex = 3;
            // 
            // cbTablet
            // 
            this.cbTablet.AutoSize = true;
            this.cbTablet.Checked = true;
            this.cbTablet.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTablet.Location = new System.Drawing.Point(8, 90);
            this.cbTablet.Name = "cbTablet";
            this.cbTablet.Size = new System.Drawing.Size(93, 29);
            this.cbTablet.TabIndex = 2;
            this.cbTablet.Text = "Tablet";
            this.cbTablet.UseVisualStyleBackColor = true;
            // 
            // numMobileWidth
            // 
            this.numMobileWidth.Location = new System.Drawing.Point(124, 46);
            this.numMobileWidth.Name = "numMobileWidth";
            this.numMobileWidth.Size = new System.Drawing.Size(120, 29);
            this.numMobileWidth.TabIndex = 1;
            // 
            // cbMobile
            // 
            this.cbMobile.AutoSize = true;
            this.cbMobile.Checked = true;
            this.cbMobile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMobile.Location = new System.Drawing.Point(8, 46);
            this.cbMobile.Name = "cbMobile";
            this.cbMobile.Size = new System.Drawing.Size(96, 29);
            this.cbMobile.TabIndex = 0;
            this.cbMobile.Text = "Mobile";
            this.cbMobile.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "json";
            this.openFileDialog1.FileName = "settings";
            this.openFileDialog1.Filter = "JSON Files (*.json)|*.json";
            // 
            // btnViewResults
            // 
            this.btnViewResults.Location = new System.Drawing.Point(492, 46);
            this.btnViewResults.Name = "btnViewResults";
            this.btnViewResults.Size = new System.Drawing.Size(295, 52);
            this.btnViewResults.TabIndex = 11;
            this.btnViewResults.Text = "View Existing Results";
            this.btnViewResults.UseVisualStyleBackColor = true;
            this.btnViewResults.Click += new System.EventHandler(this.btnViewResults_Click);
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
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 29);
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 28);
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
            this.btnCancel.Size = new System.Drawing.Size(315, 40);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            ((System.ComponentModel.ISupportInitialize)(this.numDesktop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTablet)).EndInit();
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
        private System.Windows.Forms.NumericUpDown numDesktop;
        private System.Windows.Forms.CheckBox cbDesktop;
        private System.Windows.Forms.NumericUpDown numTablet;
        private System.Windows.Forms.CheckBox cbTablet;
        private System.Windows.Forms.NumericUpDown numMobileWidth;
        private System.Windows.Forms.CheckBox cbMobile;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChooseOutput;
        private System.Windows.Forms.TextBox txtOutputDir;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnStartScreenshots;
        private System.Windows.Forms.Button btnViewResults;
        private System.Windows.Forms.OpenFileDialog openManifestDialog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem btnCancel;
    }
}

