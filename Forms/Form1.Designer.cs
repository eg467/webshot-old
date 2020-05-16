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
            this.txtSpiderSeedUris = new System.Windows.Forms.TextBox();
            this.pnlCrawl = new System.Windows.Forms.GroupBox();
            this.lblSpiderDescription = new System.Windows.Forms.Label();
            this.btnStartCrawl = new System.Windows.Forms.Button();
            this.lblSeedUrls = new System.Windows.Forms.Label();
            this.pnlSelectedPages = new System.Windows.Forms.GroupBox();
            this.clbSelectedUris = new System.Windows.Forms.CheckedListBox();
            this.btnStartScreenshots = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveProjectFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openProjectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnNewProject = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpenProject = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveProject = new System.Windows.Forms.ToolStripMenuItem();
            this.btnViewProjectFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRecentProjects = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCancelTask = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlProject = new System.Windows.Forms.GroupBox();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.lnkProjectLocation = new System.Windows.Forms.LinkLabel();
            this.folderOpenProject = new System.Windows.Forms.FolderBrowserDialog();
            this.btnShowResults = new System.Windows.Forms.ToolStripMenuItem();
            this.lblProjectSaved = new System.Windows.Forms.Label();
            this.pnlCrawl.SuspendLayout();
            this.pnlSelectedPages.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.pnlProject.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSpiderSeedUris
            // 
            this.txtSpiderSeedUris.Location = new System.Drawing.Point(124, 64);
            this.txtSpiderSeedUris.Multiline = true;
            this.txtSpiderSeedUris.Name = "txtSpiderSeedUris";
            this.txtSpiderSeedUris.Size = new System.Drawing.Size(631, 120);
            this.txtSpiderSeedUris.TabIndex = 0;
            // 
            // pnlCrawl
            // 
            this.pnlCrawl.Controls.Add(this.lblSpiderDescription);
            this.pnlCrawl.Controls.Add(this.btnStartCrawl);
            this.pnlCrawl.Controls.Add(this.lblSeedUrls);
            this.pnlCrawl.Controls.Add(this.txtSpiderSeedUris);
            this.pnlCrawl.Location = new System.Drawing.Point(12, 188);
            this.pnlCrawl.Name = "pnlCrawl";
            this.pnlCrawl.Size = new System.Drawing.Size(784, 244);
            this.pnlCrawl.TabIndex = 1;
            this.pnlCrawl.TabStop = false;
            this.pnlCrawl.Text = "Crawl Pages";
            // 
            // lblSpiderDescription
            // 
            this.lblSpiderDescription.AutoSize = true;
            this.lblSpiderDescription.Location = new System.Drawing.Point(6, 25);
            this.lblSpiderDescription.Name = "lblSpiderDescription";
            this.lblSpiderDescription.Size = new System.Drawing.Size(762, 25);
            this.lblSpiderDescription.TabIndex = 5;
            this.lblSpiderDescription.Text = "This will recursively search for all the web pages in your site based on your HTM" +
    "L links.";
            // 
            // btnStartCrawl
            // 
            this.btnStartCrawl.Location = new System.Drawing.Point(124, 192);
            this.btnStartCrawl.Name = "btnStartCrawl";
            this.btnStartCrawl.Size = new System.Drawing.Size(631, 46);
            this.btnStartCrawl.TabIndex = 4;
            this.btnStartCrawl.Text = "Find Web Pages in Site";
            this.btnStartCrawl.UseVisualStyleBackColor = true;
            this.btnStartCrawl.Click += new System.EventHandler(this.BtnStartCrawl_Click);
            // 
            // lblSeedUrls
            // 
            this.lblSeedUrls.AutoSize = true;
            this.lblSeedUrls.Location = new System.Drawing.Point(6, 102);
            this.lblSeedUrls.Name = "lblSeedUrls";
            this.lblSeedUrls.Size = new System.Drawing.Size(112, 25);
            this.lblSeedUrls.TabIndex = 1;
            this.lblSeedUrls.Text = "Seed URLs";
            // 
            // pnlSelectedPages
            // 
            this.pnlSelectedPages.Controls.Add(this.clbSelectedUris);
            this.pnlSelectedPages.Controls.Add(this.btnStartScreenshots);
            this.pnlSelectedPages.Location = new System.Drawing.Point(12, 438);
            this.pnlSelectedPages.Name = "pnlSelectedPages";
            this.pnlSelectedPages.Size = new System.Drawing.Size(784, 428);
            this.pnlSelectedPages.TabIndex = 2;
            this.pnlSelectedPages.TabStop = false;
            this.pnlSelectedPages.Text = "Selected Pages";
            // 
            // clbSelectedUris
            // 
            this.clbSelectedUris.CheckOnClick = true;
            this.clbSelectedUris.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbSelectedUris.FormattingEnabled = true;
            this.clbSelectedUris.Location = new System.Drawing.Point(3, 25);
            this.clbSelectedUris.Name = "clbSelectedUris";
            this.clbSelectedUris.Size = new System.Drawing.Size(778, 329);
            this.clbSelectedUris.TabIndex = 2;
            // 
            // btnStartScreenshots
            // 
            this.btnStartScreenshots.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnStartScreenshots.Location = new System.Drawing.Point(3, 354);
            this.btnStartScreenshots.Name = "btnStartScreenshots";
            this.btnStartScreenshots.Size = new System.Drawing.Size(778, 71);
            this.btnStartScreenshots.TabIndex = 1;
            this.btnStartScreenshots.Text = "Start Screenshots";
            this.btnStartScreenshots.UseVisualStyleBackColor = true;
            this.btnStartScreenshots.Click += new System.EventHandler(this.BtnStartScreenshots_Click);
            // 
            // saveProjectFileDialog
            // 
            this.saveProjectFileDialog.FileName = "settings.json";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "json";
            this.openFileDialog1.FileName = "settings";
            this.openFileDialog1.Filter = "JSON Files (*.json)|*.json";
            // 
            // openProjectFileDialog
            // 
            this.openProjectFileDialog.DefaultExt = "json";
            this.openProjectFileDialog.FileName = "project.wsproj";
            this.openProjectFileDialog.Filter = "Webshot Project|project.wsproj";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar,
            this.lblStatus,
            this.toolStripSplitButton1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1064);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(830, 38);
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
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(106, 34);
            this.toolStripSplitButton1.Text = "Actions";
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.btnShowResults,
            this.btnCancelTask});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(830, 38);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pToolStripMenuItem,
            this.btnExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(62, 34);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // pToolStripMenuItem
            // 
            this.pToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewProject,
            this.btnOpenProject,
            this.btnSaveProject,
            this.btnViewProjectFolder,
            this.btnRecentProjects});
            this.pToolStripMenuItem.Name = "pToolStripMenuItem";
            this.pToolStripMenuItem.Size = new System.Drawing.Size(195, 40);
            this.pToolStripMenuItem.Text = "Project";
            // 
            // btnNewProject
            // 
            this.btnNewProject.Name = "btnNewProject";
            this.btnNewProject.Size = new System.Drawing.Size(349, 40);
            this.btnNewProject.Text = "New Project";
            // 
            // btnOpenProject
            // 
            this.btnOpenProject.Name = "btnOpenProject";
            this.btnOpenProject.Size = new System.Drawing.Size(349, 40);
            this.btnOpenProject.Text = "Open Existing Project...";
            this.btnOpenProject.Click += new System.EventHandler(this.BtnOpenProject_Click);
            // 
            // btnSaveProject
            // 
            this.btnSaveProject.Name = "btnSaveProject";
            this.btnSaveProject.Size = new System.Drawing.Size(349, 40);
            this.btnSaveProject.Text = "Save Project...";
            this.btnSaveProject.Click += new System.EventHandler(this.BtnSaveProject_Click);
            // 
            // btnViewProjectFolder
            // 
            this.btnViewProjectFolder.Name = "btnViewProjectFolder";
            this.btnViewProjectFolder.Size = new System.Drawing.Size(349, 40);
            this.btnViewProjectFolder.Text = "View Project in Explorer";
            this.btnViewProjectFolder.Click += new System.EventHandler(this.BtnViewProjectFolder_Click);
            // 
            // btnRecentProjects
            // 
            this.btnRecentProjects.Name = "btnRecentProjects";
            this.btnRecentProjects.Size = new System.Drawing.Size(349, 40);
            this.btnRecentProjects.Text = "Recent Projects";
            // 
            // btnExit
            // 
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(195, 40);
            this.btnExit.Text = "Exit";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(104, 34);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsToolStripMenuItem_Click);
            // 
            // btnCancelTask
            // 
            this.btnCancelTask.Name = "btnCancelTask";
            this.btnCancelTask.Size = new System.Drawing.Size(213, 34);
            this.btnCancelTask.Text = "Cancel Current Task";
            this.btnCancelTask.Click += new System.EventHandler(this.BtnCancelTask_Click);
            // 
            // pnlProject
            // 
            this.pnlProject.Controls.Add(this.lblProjectSaved);
            this.pnlProject.Controls.Add(this.txtProjectName);
            this.pnlProject.Controls.Add(this.lnkProjectLocation);
            this.pnlProject.Location = new System.Drawing.Point(13, 42);
            this.pnlProject.Name = "pnlProject";
            this.pnlProject.Size = new System.Drawing.Size(783, 140);
            this.pnlProject.TabIndex = 6;
            this.pnlProject.TabStop = false;
            this.pnlProject.Text = "Project";
            // 
            // txtProjectName
            // 
            this.txtProjectName.Location = new System.Drawing.Point(23, 39);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(381, 29);
            this.txtProjectName.TabIndex = 3;
            this.txtProjectName.TextChanged += new System.EventHandler(this.TxtProjectName_TextChanged);
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
            this.lnkProjectLocation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkProjectLocation_LinkClicked);
            // 
            // btnShowResults
            // 
            this.btnShowResults.Name = "btnShowResults";
            this.btnShowResults.Size = new System.Drawing.Size(152, 34);
            this.btnShowResults.Text = "Show Results";
            this.btnShowResults.Click += new System.EventHandler(this.btnShowResults_Click);
            // 
            // lblProjectSaved
            // 
            this.lblProjectSaved.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblProjectSaved.Location = new System.Drawing.Point(596, 16);
            this.lblProjectSaved.Name = "lblProjectSaved";
            this.lblProjectSaved.Size = new System.Drawing.Size(171, 52);
            this.lblProjectSaved.TabIndex = 7;
            this.lblProjectSaved.Text = "Project Saved.";
            this.lblProjectSaved.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProjectSaved.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 1102);
            this.Controls.Add(this.pnlProject);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pnlSelectedPages);
            this.Controls.Add(this.pnlCrawl);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Webshot";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlCrawl.ResumeLayout(false);
            this.pnlCrawl.PerformLayout();
            this.pnlSelectedPages.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlProject.ResumeLayout(false);
            this.pnlProject.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSpiderSeedUris;
        private System.Windows.Forms.GroupBox pnlCrawl;
        private System.Windows.Forms.Label lblSeedUrls;
        private System.Windows.Forms.Button btnStartCrawl;
        private System.Windows.Forms.GroupBox pnlSelectedPages;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SaveFileDialog saveProjectFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnStartScreenshots;
        private System.Windows.Forms.OpenFileDialog openProjectFileDialog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnSaveProject;
        private System.Windows.Forms.ToolStripMenuItem btnOpenProject;
        private System.Windows.Forms.ToolStripMenuItem btnViewProjectFolder;
        private System.Windows.Forms.ToolStripMenuItem btnNewProject;
        private System.Windows.Forms.ToolStripMenuItem btnCancelTask;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.Label lblSpiderDescription;
        private System.Windows.Forms.GroupBox pnlProject;
        private System.Windows.Forms.ToolStripMenuItem btnRecentProjects;
        private System.Windows.Forms.ToolStripMenuItem btnExit;
        private System.Windows.Forms.LinkLabel lnkProjectLocation;
        private System.Windows.Forms.CheckedListBox clbSelectedUris;
        private System.Windows.Forms.FolderBrowserDialog folderOpenProject;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.ToolStripMenuItem btnShowResults;
        private System.Windows.Forms.Label lblProjectSaved;
    }
}

