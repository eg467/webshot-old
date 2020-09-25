namespace Webshot.Controls
{
    partial class ScreenshotProjectControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lnkProjectLocation = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lnkProjectName = new System.Windows.Forms.Label();
            this.clbSelectedUris = new System.Windows.Forms.CheckedListBox();
            this.btnBrokenLinks = new System.Windows.Forms.Button();
            this.btnTakeScreenshots = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCrawlPages = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSpiderSeedUris = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbSpiderIncludeExternalDomains = new System.Windows.Forms.CheckBox();
            this.cbSpiderFollowLinks = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkProjectLocation
            // 
            this.lnkProjectLocation.AutoSize = true;
            this.lnkProjectLocation.Location = new System.Drawing.Point(18, 86);
            this.lnkProjectLocation.Name = "lnkProjectLocation";
            this.lnkProjectLocation.Size = new System.Drawing.Size(117, 25);
            this.lnkProjectLocation.TabIndex = 2;
            this.lnkProjectLocation.TabStop = true;
            this.lnkProjectLocation.Text = "Project Path";
            this.lnkProjectLocation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkProjectLocation_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lnkProjectName);
            this.groupBox1.Controls.Add(this.lnkProjectLocation);
            this.groupBox1.Location = new System.Drawing.Point(30, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(783, 140);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Project";
            // 
            // lnkProjectName
            // 
            this.lnkProjectName.AutoSize = true;
            this.lnkProjectName.Location = new System.Drawing.Point(23, 40);
            this.lnkProjectName.Name = "lnkProjectName";
            this.lnkProjectName.Size = new System.Drawing.Size(129, 25);
            this.lnkProjectName.TabIndex = 3;
            this.lnkProjectName.Text = "Project Name";
            // 
            // clbSelectedUris
            // 
            this.clbSelectedUris.CheckOnClick = true;
            this.clbSelectedUris.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbSelectedUris.FormattingEnabled = true;
            this.clbSelectedUris.IntegralHeight = false;
            this.clbSelectedUris.Location = new System.Drawing.Point(3, 86);
            this.clbSelectedUris.Name = "clbSelectedUris";
            this.clbSelectedUris.Size = new System.Drawing.Size(778, 194);
            this.clbSelectedUris.TabIndex = 2;
            this.clbSelectedUris.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbSelectedUris_ItemCheck);
            // 
            // btnBrokenLinks
            // 
            this.btnBrokenLinks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnBrokenLinks.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBrokenLinks.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrokenLinks.Location = new System.Drawing.Point(3, 25);
            this.btnBrokenLinks.Name = "btnBrokenLinks";
            this.btnBrokenLinks.Size = new System.Drawing.Size(778, 61);
            this.btnBrokenLinks.TabIndex = 3;
            this.btnBrokenLinks.Text = "View Broken Links";
            this.btnBrokenLinks.UseVisualStyleBackColor = false;
            this.btnBrokenLinks.Click += new System.EventHandler(this.btnBrokenLinks_Click);
            // 
            // btnTakeScreenshots
            // 
            this.btnTakeScreenshots.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnTakeScreenshots.Location = new System.Drawing.Point(3, 280);
            this.btnTakeScreenshots.Name = "btnTakeScreenshots";
            this.btnTakeScreenshots.Size = new System.Drawing.Size(778, 71);
            this.btnTakeScreenshots.TabIndex = 1;
            this.btnTakeScreenshots.Text = "Start Screenshots";
            this.btnTakeScreenshots.UseVisualStyleBackColor = true;
            this.btnTakeScreenshots.Click += new System.EventHandler(this.btnTakeScreenshots_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.clbSelectedUris);
            this.groupBox2.Controls.Add(this.btnBrokenLinks);
            this.groupBox2.Controls.Add(this.btnTakeScreenshots);
            this.groupBox2.Location = new System.Drawing.Point(29, 573);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(784, 354);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected Pages";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(762, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "This will recursively search for all the web pages in your site based on your HTM" +
    "L links.";
            // 
            // btnCrawlPages
            // 
            this.btnCrawlPages.Location = new System.Drawing.Point(124, 275);
            this.btnCrawlPages.Name = "btnCrawlPages";
            this.btnCrawlPages.Size = new System.Drawing.Size(631, 59);
            this.btnCrawlPages.TabIndex = 4;
            this.btnCrawlPages.Text = "Find Web Pages in Site";
            this.btnCrawlPages.UseVisualStyleBackColor = true;
            this.btnCrawlPages.Click += new System.EventHandler(this.btnCrawlPages_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "Seed URLs";
            // 
            // txtSpiderSeedUris
            // 
            this.txtSpiderSeedUris.Location = new System.Drawing.Point(124, 64);
            this.txtSpiderSeedUris.Multiline = true;
            this.txtSpiderSeedUris.Name = "txtSpiderSeedUris";
            this.txtSpiderSeedUris.Size = new System.Drawing.Size(631, 120);
            this.txtSpiderSeedUris.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbSpiderIncludeExternalDomains);
            this.groupBox3.Controls.Add(this.cbSpiderFollowLinks);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.btnCrawlPages);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txtSpiderSeedUris);
            this.groupBox3.Location = new System.Drawing.Point(29, 182);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(784, 357);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Crawl Pages";
            // 
            // cbSpiderIncludeExternalDomains
            // 
            this.cbSpiderIncludeExternalDomains.AutoSize = true;
            this.cbSpiderIncludeExternalDomains.Location = new System.Drawing.Point(124, 226);
            this.cbSpiderIncludeExternalDomains.Name = "cbSpiderIncludeExternalDomains";
            this.cbSpiderIncludeExternalDomains.Size = new System.Drawing.Size(259, 29);
            this.cbSpiderIncludeExternalDomains.TabIndex = 7;
            this.cbSpiderIncludeExternalDomains.Text = "Include External Domains";
            this.cbSpiderIncludeExternalDomains.UseVisualStyleBackColor = true;
            // 
            // cbSpiderFollowLinks
            // 
            this.cbSpiderFollowLinks.AutoSize = true;
            this.cbSpiderFollowLinks.Location = new System.Drawing.Point(124, 191);
            this.cbSpiderFollowLinks.Name = "cbSpiderFollowLinks";
            this.cbSpiderFollowLinks.Size = new System.Drawing.Size(145, 29);
            this.cbSpiderFollowLinks.TabIndex = 6;
            this.cbSpiderFollowLinks.Text = "Follow Links";
            this.cbSpiderFollowLinks.UseVisualStyleBackColor = true;
            // 
            // ScreenshotProjectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "ScreenshotProjectControl";
            this.Size = new System.Drawing.Size(862, 1008);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.LinkLabel lnkProjectLocation;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox clbSelectedUris;
        private System.Windows.Forms.Button btnBrokenLinks;
        private System.Windows.Forms.Button btnTakeScreenshots;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCrawlPages;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSpiderSeedUris;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbSpiderIncludeExternalDomains;
        private System.Windows.Forms.CheckBox cbSpiderFollowLinks;
        private System.Windows.Forms.Label lnkProjectName;
    }
}
