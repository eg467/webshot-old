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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveProjectFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openProjectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.lblProjectSaved = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnNewProject = new System.Windows.Forms.ToolStripMenuItem();
            this.btnQuickCreateProject = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCreateProject = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpenProject = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveProject = new System.Windows.Forms.ToolStripMenuItem();
            this.btnViewProjectFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRecentProjects = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnShowResults = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCancelTask = new System.Windows.Forms.ToolStripMenuItem();
            this.folderOpenProject = new System.Windows.Forms.FolderBrowserDialog();
            this.pnlProjectMain = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabManualScreenshotProject = new System.Windows.Forms.TabPage();
            this.projectControl = new Webshot.Controls.ScreenshotProjectControl();
            this.tabScheduledScreenshots = new System.Windows.Forms.TabPage();
            this.automatedScreenshotsControl1 = new Webshot.Controls.AutomatedScreenshotsControl();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.pnlProjectMain.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabManualScreenshotProject.SuspendLayout();
            this.tabScheduledScreenshots.SuspendLayout();
            this.SuspendLayout();
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
            this.toolStripSplitButton1,
            this.lblProjectSaved});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1345);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(2143, 47);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 37);
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 38);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(106, 43);
            this.toolStripSplitButton1.Text = "Actions";
            // 
            // lblProjectSaved
            // 
            this.lblProjectSaved.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblProjectSaved.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblProjectSaved.Name = "lblProjectSaved";
            this.lblProjectSaved.Padding = new System.Windows.Forms.Padding(5);
            this.lblProjectSaved.Size = new System.Drawing.Size(205, 48);
            this.lblProjectSaved.Text = "Project Saved";
            this.lblProjectSaved.Visible = false;
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
            this.menuStrip1.Size = new System.Drawing.Size(2143, 38);
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
            this.btnNewProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnQuickCreateProject,
            this.btnCreateProject});
            this.btnNewProject.Name = "btnNewProject";
            this.btnNewProject.Size = new System.Drawing.Size(349, 40);
            this.btnNewProject.Text = "New Project";
            // 
            // btnQuickCreateProject
            // 
            this.btnQuickCreateProject.Name = "btnQuickCreateProject";
            this.btnQuickCreateProject.Size = new System.Drawing.Size(290, 40);
            this.btnQuickCreateProject.Text = "Quick Create";
            this.btnQuickCreateProject.Click += new System.EventHandler(this.BtnQuickCreateProject_Click);
            // 
            // btnCreateProject
            // 
            this.btnCreateProject.Name = "btnCreateProject";
            this.btnCreateProject.Size = new System.Drawing.Size(290, 40);
            this.btnCreateProject.Text = "Choose Directory";
            this.btnCreateProject.Click += new System.EventHandler(this.BtnCreateProject_Click);
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
            // btnShowResults
            // 
            this.btnShowResults.Name = "btnShowResults";
            this.btnShowResults.Size = new System.Drawing.Size(152, 34);
            this.btnShowResults.Text = "Show Results";
            this.btnShowResults.Click += new System.EventHandler(this.BtnShowResults_Click);
            // 
            // btnCancelTask
            // 
            this.btnCancelTask.Name = "btnCancelTask";
            this.btnCancelTask.Size = new System.Drawing.Size(213, 34);
            this.btnCancelTask.Text = "Cancel Current Task";
            this.btnCancelTask.Click += new System.EventHandler(this.BtnCancelTask_Click);
            // 
            // pnlProjectMain
            // 
            this.pnlProjectMain.Controls.Add(this.tabControl1);
            this.pnlProjectMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProjectMain.Location = new System.Drawing.Point(0, 38);
            this.pnlProjectMain.Name = "pnlProjectMain";
            this.pnlProjectMain.Size = new System.Drawing.Size(2143, 1307);
            this.pnlProjectMain.TabIndex = 7;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabManualScreenshotProject);
            this.tabControl1.Controls.Add(this.tabScheduledScreenshots);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(2143, 1307);
            this.tabControl1.TabIndex = 1;
            // 
            // tabManualScreenshotProject
            // 
            this.tabManualScreenshotProject.Controls.Add(this.projectControl);
            this.tabManualScreenshotProject.Location = new System.Drawing.Point(4, 33);
            this.tabManualScreenshotProject.Name = "tabManualScreenshotProject";
            this.tabManualScreenshotProject.Padding = new System.Windows.Forms.Padding(3);
            this.tabManualScreenshotProject.Size = new System.Drawing.Size(2135, 1270);
            this.tabManualScreenshotProject.TabIndex = 0;
            this.tabManualScreenshotProject.Text = "Manual Project Control";
            this.tabManualScreenshotProject.UseVisualStyleBackColor = true;
            // 
            // projectControl
            // 
            this.projectControl.AutoSize = true;
            this.projectControl.Busy = false;
            this.projectControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectControl.Location = new System.Drawing.Point(3, 3);
            this.projectControl.Name = "projectControl";
            this.projectControl.Progress = null;
            this.projectControl.Project = null;
            this.projectControl.ProjectNameUi = "";
            this.projectControl.ProjectPathUi = "Project Path";
            this.projectControl.Size = new System.Drawing.Size(2129, 1264);
            this.projectControl.SpiderSeedUrisUi = ((System.Collections.Generic.IEnumerable<System.Uri>)(resources.GetObject("projectControl.SpiderSeedUrisUi")));
            this.projectControl.TabIndex = 0;
            this.projectControl.Visible = false;
            // 
            // tabScheduledScreenshots
            // 
            this.tabScheduledScreenshots.Controls.Add(this.automatedScreenshotsControl1);
            this.tabScheduledScreenshots.Location = new System.Drawing.Point(4, 33);
            this.tabScheduledScreenshots.Name = "tabScheduledScreenshots";
            this.tabScheduledScreenshots.Padding = new System.Windows.Forms.Padding(3);
            this.tabScheduledScreenshots.Size = new System.Drawing.Size(2135, 1270);
            this.tabScheduledScreenshots.TabIndex = 1;
            this.tabScheduledScreenshots.Text = "Scheduled Screenshots";
            this.tabScheduledScreenshots.UseVisualStyleBackColor = true;
            // 
            // automatedScreenshotsControl1
            // 
            this.automatedScreenshotsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.automatedScreenshotsControl1.Location = new System.Drawing.Point(3, 3);
            this.automatedScreenshotsControl1.Name = "automatedScreenshotsControl1";
            this.automatedScreenshotsControl1.Progress = null;
            this.automatedScreenshotsControl1.Size = new System.Drawing.Size(2129, 1264);
            this.automatedScreenshotsControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2143, 1392);
            this.Controls.Add(this.pnlProjectMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Webshot";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlProjectMain.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabManualScreenshotProject.ResumeLayout(false);
            this.tabManualScreenshotProject.PerformLayout();
            this.tabScheduledScreenshots.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SaveFileDialog saveProjectFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
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
        private System.Windows.Forms.ToolStripMenuItem btnRecentProjects;
        private System.Windows.Forms.ToolStripMenuItem btnExit;
        private System.Windows.Forms.FolderBrowserDialog folderOpenProject;
        private System.Windows.Forms.ToolStripMenuItem btnShowResults;
        private System.Windows.Forms.ToolStripMenuItem btnQuickCreateProject;
        private System.Windows.Forms.ToolStripMenuItem btnCreateProject;
        private System.Windows.Forms.Panel pnlProjectMain;
        private Controls.ScreenshotProjectControl projectControl;
        private System.Windows.Forms.ToolStripStatusLabel lblProjectSaved;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabManualScreenshotProject;
        private System.Windows.Forms.TabPage tabScheduledScreenshots;
        private Controls.AutomatedScreenshotsControl automatedScreenshotsControl1;
    }
}

