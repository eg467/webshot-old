namespace Webshot.Forms
{
    partial class OptionsForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlBasicAuth = new System.Windows.Forms.GroupBox();
            this.lblCredsWarning = new System.Windows.Forms.Label();
            this.lblCredsInstructions = new System.Windows.Forms.Label();
            this.txtCreds = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnResetOptions = new System.Windows.Forms.Button();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.tabsSettings = new System.Windows.Forms.TabControl();
            this.tabSpider = new System.Windows.Forms.TabPage();
            this.txtSpiderBlacklist = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCrawlExternalLinks = new System.Windows.Forms.CheckBox();
            this.tabDisplay = new System.Windows.Forms.TabPage();
            this.cbConstrainWidth = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDesktopZoom = new System.Windows.Forms.Label();
            this.lblTabletZoom = new System.Windows.Forms.Label();
            this.lblMobileZoom = new System.Windows.Forms.Label();
            this.numMobileZoom = new System.Windows.Forms.NumericUpDown();
            this.numTabletZoom = new System.Windows.Forms.NumericUpDown();
            this.numDesktopZoom = new System.Windows.Forms.NumericUpDown();
            this.tabScreenshots = new System.Windows.Forms.TabPage();
            this.cbStoreVersions = new System.Windows.Forms.CheckBox();
            this.pnlDeviceWidths = new System.Windows.Forms.GroupBox();
            this.cbDesktop = new System.Windows.Forms.CheckBox();
            this.cbTablet = new System.Windows.Forms.CheckBox();
            this.cbMobile = new System.Windows.Forms.CheckBox();
            this.numMobileWidth = new System.Windows.Forms.NumericUpDown();
            this.numTabletWidth = new System.Windows.Forms.NumericUpDown();
            this.numDesktopWidth = new System.Windows.Forms.NumericUpDown();
            this.tabScheduler = new System.Windows.Forms.TabPage();
            this.cbScheduled = new System.Windows.Forms.CheckBox();
            this.numScheduleInterval = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.pnlBasicAuth.SuspendLayout();
            this.tabsSettings.SuspendLayout();
            this.tabSpider.SuspendLayout();
            this.tabDisplay.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMobileZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTabletZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDesktopZoom)).BeginInit();
            this.tabScreenshots.SuspendLayout();
            this.pnlDeviceWidths.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMobileWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTabletWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDesktopWidth)).BeginInit();
            this.tabScheduler.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScheduleInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlBasicAuth);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnResetOptions);
            this.panel1.Controls.Add(this.lblProjectName);
            this.panel1.Controls.Add(this.txtProjectName);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 693);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1284, 357);
            this.panel1.TabIndex = 0;
            // 
            // pnlBasicAuth
            // 
            this.pnlBasicAuth.Controls.Add(this.lblCredsWarning);
            this.pnlBasicAuth.Controls.Add(this.lblCredsInstructions);
            this.pnlBasicAuth.Controls.Add(this.txtCreds);
            this.pnlBasicAuth.Location = new System.Drawing.Point(149, 6);
            this.pnlBasicAuth.Name = "pnlBasicAuth";
            this.pnlBasicAuth.Size = new System.Drawing.Size(589, 265);
            this.pnlBasicAuth.TabIndex = 5;
            this.pnlBasicAuth.TabStop = false;
            this.pnlBasicAuth.Text = "BasicAuthentication";
            // 
            // lblCredsWarning
            // 
            this.lblCredsWarning.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblCredsWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredsWarning.Location = new System.Drawing.Point(7, 53);
            this.lblCredsWarning.Name = "lblCredsWarning";
            this.lblCredsWarning.Size = new System.Drawing.Size(576, 56);
            this.lblCredsWarning.TabIndex = 2;
            this.lblCredsWarning.Text = "WARNING: THESE WILL BE STORED IN PLAIN TEXT ON YOUR DISK. THIS ISN\'T TECHNICALLY " +
    "SECURE.";
            // 
            // lblCredsInstructions
            // 
            this.lblCredsInstructions.AutoSize = true;
            this.lblCredsInstructions.Location = new System.Drawing.Point(7, 26);
            this.lblCredsInstructions.Name = "lblCredsInstructions";
            this.lblCredsInstructions.Size = new System.Drawing.Size(550, 25);
            this.lblCredsInstructions.TabIndex = 1;
            this.lblCredsInstructions.Text = "Enter in form \'domain:user:password\', one per line per domain.";
            // 
            // txtCreds
            // 
            this.txtCreds.Location = new System.Drawing.Point(6, 112);
            this.txtCreds.Multiline = true;
            this.txtCreds.Name = "txtCreds";
            this.txtCreds.Size = new System.Drawing.Size(577, 141);
            this.txtCreds.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "User";
            // 
            // btnResetOptions
            // 
            this.btnResetOptions.Location = new System.Drawing.Point(642, 291);
            this.btnResetOptions.Name = "btnResetOptions";
            this.btnResetOptions.Size = new System.Drawing.Size(155, 38);
            this.btnResetOptions.TabIndex = 3;
            this.btnResetOptions.Text = "Reset";
            this.btnResetOptions.UseVisualStyleBackColor = true;
            this.btnResetOptions.Click += new System.EventHandler(this.BtnResetOptions_Click);
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Location = new System.Drawing.Point(15, 298);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(129, 25);
            this.lblProjectName.TabIndex = 2;
            this.lblProjectName.Text = "Project Name";
            // 
            // txtProjectName
            // 
            this.txtProjectName.Location = new System.Drawing.Point(150, 295);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(321, 29);
            this.txtProjectName.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(477, 291);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(159, 38);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // tabsSettings
            // 
            this.tabsSettings.Controls.Add(this.tabSpider);
            this.tabsSettings.Controls.Add(this.tabDisplay);
            this.tabsSettings.Controls.Add(this.tabScreenshots);
            this.tabsSettings.Controls.Add(this.tabScheduler);
            this.tabsSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabsSettings.Location = new System.Drawing.Point(0, 0);
            this.tabsSettings.Name = "tabsSettings";
            this.tabsSettings.SelectedIndex = 0;
            this.tabsSettings.Size = new System.Drawing.Size(1284, 693);
            this.tabsSettings.TabIndex = 1;
            // 
            // tabSpider
            // 
            this.tabSpider.Controls.Add(this.txtSpiderBlacklist);
            this.tabSpider.Controls.Add(this.label1);
            this.tabSpider.Controls.Add(this.cbCrawlExternalLinks);
            this.tabSpider.Location = new System.Drawing.Point(4, 33);
            this.tabSpider.Name = "tabSpider";
            this.tabSpider.Padding = new System.Windows.Forms.Padding(3);
            this.tabSpider.Size = new System.Drawing.Size(1276, 656);
            this.tabSpider.TabIndex = 0;
            this.tabSpider.Text = "Spider";
            this.tabSpider.UseVisualStyleBackColor = true;
            // 
            // txtSpiderBlacklist
            // 
            this.txtSpiderBlacklist.Location = new System.Drawing.Point(182, 63);
            this.txtSpiderBlacklist.Name = "txtSpiderBlacklist";
            this.txtSpiderBlacklist.Size = new System.Drawing.Size(529, 29);
            this.txtSpiderBlacklist.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Regex Blacklist";
            // 
            // cbCrawlExternalLinks
            // 
            this.cbCrawlExternalLinks.AutoSize = true;
            this.cbCrawlExternalLinks.Location = new System.Drawing.Point(21, 20);
            this.cbCrawlExternalLinks.Name = "cbCrawlExternalLinks";
            this.cbCrawlExternalLinks.Size = new System.Drawing.Size(221, 29);
            this.cbCrawlExternalLinks.TabIndex = 4;
            this.cbCrawlExternalLinks.Text = "Follow External Links";
            this.cbCrawlExternalLinks.UseVisualStyleBackColor = true;
            // 
            // tabDisplay
            // 
            this.tabDisplay.Controls.Add(this.cbConstrainWidth);
            this.tabDisplay.Controls.Add(this.groupBox1);
            this.tabDisplay.Location = new System.Drawing.Point(4, 33);
            this.tabDisplay.Name = "tabDisplay";
            this.tabDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.tabDisplay.Size = new System.Drawing.Size(1276, 656);
            this.tabDisplay.TabIndex = 1;
            this.tabDisplay.Text = "Display";
            this.tabDisplay.UseVisualStyleBackColor = true;
            // 
            // cbConstrainWidth
            // 
            this.cbConstrainWidth.AutoSize = true;
            this.cbConstrainWidth.Location = new System.Drawing.Point(15, 212);
            this.cbConstrainWidth.Name = "cbConstrainWidth";
            this.cbConstrainWidth.Size = new System.Drawing.Size(336, 29);
            this.cbConstrainWidth.TabIndex = 18;
            this.cbConstrainWidth.Text = "Constrain image width to container";
            this.cbConstrainWidth.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDesktopZoom);
            this.groupBox1.Controls.Add(this.lblTabletZoom);
            this.groupBox1.Controls.Add(this.lblMobileZoom);
            this.groupBox1.Controls.Add(this.numMobileZoom);
            this.groupBox1.Controls.Add(this.numTabletZoom);
            this.groupBox1.Controls.Add(this.numDesktopZoom);
            this.groupBox1.Location = new System.Drawing.Point(15, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 188);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image Zoom";
            // 
            // lblDesktopZoom
            // 
            this.lblDesktopZoom.AutoSize = true;
            this.lblDesktopZoom.Location = new System.Drawing.Point(14, 132);
            this.lblDesktopZoom.Name = "lblDesktopZoom";
            this.lblDesktopZoom.Size = new System.Drawing.Size(84, 25);
            this.lblDesktopZoom.TabIndex = 20;
            this.lblDesktopZoom.Text = "Desktop";
            // 
            // lblTabletZoom
            // 
            this.lblTabletZoom.AutoSize = true;
            this.lblTabletZoom.Location = new System.Drawing.Point(14, 88);
            this.lblTabletZoom.Name = "lblTabletZoom";
            this.lblTabletZoom.Size = new System.Drawing.Size(67, 25);
            this.lblTabletZoom.TabIndex = 19;
            this.lblTabletZoom.Text = "Tablet";
            // 
            // lblMobileZoom
            // 
            this.lblMobileZoom.AutoSize = true;
            this.lblMobileZoom.Location = new System.Drawing.Point(14, 44);
            this.lblMobileZoom.Name = "lblMobileZoom";
            this.lblMobileZoom.Size = new System.Drawing.Size(70, 25);
            this.lblMobileZoom.TabIndex = 18;
            this.lblMobileZoom.Text = "Mobile";
            // 
            // numMobileZoom
            // 
            this.numMobileZoom.Location = new System.Drawing.Point(104, 42);
            this.numMobileZoom.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numMobileZoom.Name = "numMobileZoom";
            this.numMobileZoom.Size = new System.Drawing.Size(120, 29);
            this.numMobileZoom.TabIndex = 1;
            this.numMobileZoom.Tag = "Mobile";
            // 
            // numTabletZoom
            // 
            this.numTabletZoom.Location = new System.Drawing.Point(104, 86);
            this.numTabletZoom.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numTabletZoom.Name = "numTabletZoom";
            this.numTabletZoom.Size = new System.Drawing.Size(120, 29);
            this.numTabletZoom.TabIndex = 3;
            this.numTabletZoom.Tag = "Tablet";
            // 
            // numDesktopZoom
            // 
            this.numDesktopZoom.Location = new System.Drawing.Point(104, 130);
            this.numDesktopZoom.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numDesktopZoom.Name = "numDesktopZoom";
            this.numDesktopZoom.Size = new System.Drawing.Size(120, 29);
            this.numDesktopZoom.TabIndex = 5;
            this.numDesktopZoom.Tag = "Desktop";
            // 
            // tabScreenshots
            // 
            this.tabScreenshots.Controls.Add(this.cbStoreVersions);
            this.tabScreenshots.Controls.Add(this.pnlDeviceWidths);
            this.tabScreenshots.Location = new System.Drawing.Point(4, 33);
            this.tabScreenshots.Name = "tabScreenshots";
            this.tabScreenshots.Padding = new System.Windows.Forms.Padding(3);
            this.tabScreenshots.Size = new System.Drawing.Size(1276, 656);
            this.tabScreenshots.TabIndex = 2;
            this.tabScreenshots.Text = "Screenshots";
            this.tabScreenshots.UseVisualStyleBackColor = true;
            // 
            // cbStoreVersions
            // 
            this.cbStoreVersions.AutoSize = true;
            this.cbStoreVersions.Checked = true;
            this.cbStoreVersions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbStoreVersions.Location = new System.Drawing.Point(20, 236);
            this.cbStoreVersions.Name = "cbStoreVersions";
            this.cbStoreVersions.Size = new System.Drawing.Size(303, 29);
            this.cbStoreVersions.TabIndex = 17;
            this.cbStoreVersions.Text = "Store Versions of Screenshots";
            this.cbStoreVersions.UseVisualStyleBackColor = true;
            // 
            // pnlDeviceWidths
            // 
            this.pnlDeviceWidths.Controls.Add(this.cbDesktop);
            this.pnlDeviceWidths.Controls.Add(this.cbTablet);
            this.pnlDeviceWidths.Controls.Add(this.cbMobile);
            this.pnlDeviceWidths.Controls.Add(this.numMobileWidth);
            this.pnlDeviceWidths.Controls.Add(this.numTabletWidth);
            this.pnlDeviceWidths.Controls.Add(this.numDesktopWidth);
            this.pnlDeviceWidths.Location = new System.Drawing.Point(20, 18);
            this.pnlDeviceWidths.Name = "pnlDeviceWidths";
            this.pnlDeviceWidths.Size = new System.Drawing.Size(457, 188);
            this.pnlDeviceWidths.TabIndex = 16;
            this.pnlDeviceWidths.TabStop = false;
            this.pnlDeviceWidths.Text = "Device Widths";
            // 
            // cbDesktop
            // 
            this.cbDesktop.AutoSize = true;
            this.cbDesktop.Checked = true;
            this.cbDesktop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDesktop.Location = new System.Drawing.Point(6, 139);
            this.cbDesktop.Name = "cbDesktop";
            this.cbDesktop.Size = new System.Drawing.Size(110, 29);
            this.cbDesktop.TabIndex = 17;
            this.cbDesktop.Text = "Desktop";
            this.cbDesktop.UseVisualStyleBackColor = true;
            // 
            // cbTablet
            // 
            this.cbTablet.AutoSize = true;
            this.cbTablet.Checked = true;
            this.cbTablet.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTablet.Location = new System.Drawing.Point(6, 95);
            this.cbTablet.Name = "cbTablet";
            this.cbTablet.Size = new System.Drawing.Size(93, 29);
            this.cbTablet.TabIndex = 16;
            this.cbTablet.Text = "Tablet";
            this.cbTablet.UseVisualStyleBackColor = true;
            // 
            // cbMobile
            // 
            this.cbMobile.AutoSize = true;
            this.cbMobile.Checked = true;
            this.cbMobile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMobile.Location = new System.Drawing.Point(7, 55);
            this.cbMobile.Name = "cbMobile";
            this.cbMobile.Size = new System.Drawing.Size(96, 29);
            this.cbMobile.TabIndex = 15;
            this.cbMobile.Text = "Mobile";
            this.cbMobile.UseVisualStyleBackColor = true;
            // 
            // numMobileWidth
            // 
            this.numMobileWidth.Location = new System.Drawing.Point(125, 50);
            this.numMobileWidth.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numMobileWidth.Name = "numMobileWidth";
            this.numMobileWidth.Size = new System.Drawing.Size(120, 29);
            this.numMobileWidth.TabIndex = 1;
            this.numMobileWidth.Tag = "Mobile";
            // 
            // numTabletWidth
            // 
            this.numTabletWidth.Location = new System.Drawing.Point(125, 94);
            this.numTabletWidth.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numTabletWidth.Name = "numTabletWidth";
            this.numTabletWidth.Size = new System.Drawing.Size(120, 29);
            this.numTabletWidth.TabIndex = 3;
            this.numTabletWidth.Tag = "Tablet";
            // 
            // numDesktopWidth
            // 
            this.numDesktopWidth.Location = new System.Drawing.Point(125, 138);
            this.numDesktopWidth.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numDesktopWidth.Name = "numDesktopWidth";
            this.numDesktopWidth.Size = new System.Drawing.Size(120, 29);
            this.numDesktopWidth.TabIndex = 5;
            this.numDesktopWidth.Tag = "Desktop";
            // 
            // tabScheduler
            // 
            this.tabScheduler.Controls.Add(this.label3);
            this.tabScheduler.Controls.Add(this.numScheduleInterval);
            this.tabScheduler.Controls.Add(this.cbScheduled);
            this.tabScheduler.Location = new System.Drawing.Point(4, 33);
            this.tabScheduler.Name = "tabScheduler";
            this.tabScheduler.Size = new System.Drawing.Size(1276, 656);
            this.tabScheduler.TabIndex = 3;
            this.tabScheduler.Text = "Scheduler";
            this.tabScheduler.UseVisualStyleBackColor = true;
            // 
            // cbScheduled
            // 
            this.cbScheduled.AutoSize = true;
            this.cbScheduled.Location = new System.Drawing.Point(8, 17);
            this.cbScheduled.Name = "cbScheduled";
            this.cbScheduled.Size = new System.Drawing.Size(353, 29);
            this.cbScheduled.TabIndex = 0;
            this.cbScheduled.Text = "Automate screenshots via scheduler";
            this.cbScheduled.UseVisualStyleBackColor = true;
            // 
            // numScheduleInterval
            // 
            this.numScheduleInterval.Location = new System.Drawing.Point(336, 65);
            this.numScheduleInterval.Name = "numScheduleInterval";
            this.numScheduleInterval.Size = new System.Drawing.Size(120, 29);
            this.numScheduleInterval.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(319, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Take screenshots every N minutes:";
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 1050);
            this.Controls.Add(this.tabsSettings);
            this.Controls.Add(this.panel1);
            this.Name = "OptionsForm";
            this.Text = "SettingsForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlBasicAuth.ResumeLayout(false);
            this.pnlBasicAuth.PerformLayout();
            this.tabsSettings.ResumeLayout(false);
            this.tabSpider.ResumeLayout(false);
            this.tabSpider.PerformLayout();
            this.tabDisplay.ResumeLayout(false);
            this.tabDisplay.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMobileZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTabletZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDesktopZoom)).EndInit();
            this.tabScreenshots.ResumeLayout(false);
            this.tabScreenshots.PerformLayout();
            this.pnlDeviceWidths.ResumeLayout(false);
            this.pnlDeviceWidths.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMobileWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTabletWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDesktopWidth)).EndInit();
            this.tabScheduler.ResumeLayout(false);
            this.tabScheduler.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScheduleInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabsSettings;
        private System.Windows.Forms.TabPage tabSpider;
        private System.Windows.Forms.TabPage tabDisplay;
        private System.Windows.Forms.TabPage tabScreenshots;
        private System.Windows.Forms.GroupBox pnlDeviceWidths;
        private System.Windows.Forms.NumericUpDown numMobileWidth;
        private System.Windows.Forms.NumericUpDown numTabletWidth;
        private System.Windows.Forms.NumericUpDown numDesktopWidth;
        private System.Windows.Forms.CheckBox cbCrawlExternalLinks;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.CheckBox cbStoreVersions;
        private System.Windows.Forms.CheckBox cbDesktop;
        private System.Windows.Forms.CheckBox cbTablet;
        private System.Windows.Forms.CheckBox cbMobile;
        private System.Windows.Forms.CheckBox cbConstrainWidth;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblDesktopZoom;
        private System.Windows.Forms.Label lblTabletZoom;
        private System.Windows.Forms.Label lblMobileZoom;
        private System.Windows.Forms.NumericUpDown numMobileZoom;
        private System.Windows.Forms.NumericUpDown numTabletZoom;
        private System.Windows.Forms.NumericUpDown numDesktopZoom;
        private System.Windows.Forms.TextBox txtSpiderBlacklist;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnResetOptions;
        private System.Windows.Forms.GroupBox pnlBasicAuth;
        private System.Windows.Forms.TextBox txtCreds;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCredsWarning;
        private System.Windows.Forms.Label lblCredsInstructions;
        private System.Windows.Forms.TabPage tabScheduler;
        private System.Windows.Forms.CheckBox cbScheduled;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numScheduleInterval;
    }
}