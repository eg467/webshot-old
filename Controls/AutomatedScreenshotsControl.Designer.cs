namespace Webshot.Controls
{
    partial class AutomatedScreenshotsControl
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
            this.components = new System.ComponentModel.Container();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.lvScheduledProjects = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDomains = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnOpenProjectFolder = new System.Windows.Forms.Button();
            this.pnlProjectOptions = new System.Windows.Forms.GroupBox();
            this.btnProjectOptions = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.lnkRefresh = new System.Windows.Forms.LinkLabel();
            this.lnkOpenSchedulerFile = new System.Windows.Forms.LinkLabel();
            this.colLastRun = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInterval = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNextRun = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlProjectOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartStop
            // 
            this.btnStartStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartStop.Location = new System.Drawing.Point(9, 541);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(1109, 61);
            this.btnStartStop.TabIndex = 2;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // lvScheduledProjects
            // 
            this.lvScheduledProjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colDomains,
            this.colPath,
            this.colLastRun,
            this.colInterval,
            this.colNextRun});
            this.lvScheduledProjects.FullRowSelect = true;
            this.lvScheduledProjects.HideSelection = false;
            this.lvScheduledProjects.Location = new System.Drawing.Point(9, 54);
            this.lvScheduledProjects.Name = "lvScheduledProjects";
            this.lvScheduledProjects.Size = new System.Drawing.Size(1655, 319);
            this.lvScheduledProjects.TabIndex = 3;
            this.lvScheduledProjects.UseCompatibleStateImageBehavior = false;
            this.lvScheduledProjects.View = System.Windows.Forms.View.Details;
            this.lvScheduledProjects.SelectedIndexChanged += new System.EventHandler(this.lvScheduledProjects_SelectedIndexChanged);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 179;
            // 
            // colDomains
            // 
            this.colDomains.Text = "Domains";
            this.colDomains.Width = 258;
            // 
            // colPath
            // 
            this.colPath.Text = "Path";
            this.colPath.Width = 330;
            // 
            // btnOpenProjectFolder
            // 
            this.btnOpenProjectFolder.Location = new System.Drawing.Point(6, 41);
            this.btnOpenProjectFolder.Name = "btnOpenProjectFolder";
            this.btnOpenProjectFolder.Size = new System.Drawing.Size(1112, 59);
            this.btnOpenProjectFolder.TabIndex = 4;
            this.btnOpenProjectFolder.Text = "Open Folder";
            this.btnOpenProjectFolder.UseVisualStyleBackColor = true;
            this.btnOpenProjectFolder.Click += new System.EventHandler(this.btnOpenProjectFolder_Click);
            // 
            // pnlProjectOptions
            // 
            this.pnlProjectOptions.Controls.Add(this.btnProjectOptions);
            this.pnlProjectOptions.Controls.Add(this.btnOpenProjectFolder);
            this.pnlProjectOptions.Enabled = false;
            this.pnlProjectOptions.Location = new System.Drawing.Point(0, 379);
            this.pnlProjectOptions.Name = "pnlProjectOptions";
            this.pnlProjectOptions.Size = new System.Drawing.Size(1664, 156);
            this.pnlProjectOptions.TabIndex = 5;
            this.pnlProjectOptions.TabStop = false;
            this.pnlProjectOptions.Text = "Project Options";
            // 
            // btnProjectOptions
            // 
            this.btnProjectOptions.Location = new System.Drawing.Point(6, 106);
            this.btnProjectOptions.Name = "btnProjectOptions";
            this.btnProjectOptions.Size = new System.Drawing.Size(1112, 44);
            this.btnProjectOptions.TabIndex = 5;
            this.btnProjectOptions.Text = "Options";
            this.btnProjectOptions.UseVisualStyleBackColor = true;
            this.btnProjectOptions.Click += new System.EventHandler(this.btnProjectOptions_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(9, 612);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(1109, 460);
            this.txtLog.TabIndex = 6;
            this.txtLog.Text = "";
            // 
            // lnkRefresh
            // 
            this.lnkRefresh.AutoSize = true;
            this.lnkRefresh.Location = new System.Drawing.Point(4, 4);
            this.lnkRefresh.Name = "lnkRefresh";
            this.lnkRefresh.Size = new System.Drawing.Size(79, 25);
            this.lnkRefresh.TabIndex = 7;
            this.lnkRefresh.TabStop = true;
            this.lnkRefresh.Text = "Refresh";
            this.lnkRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRefresh_LinkClicked);
            // 
            // lnkOpenSchedulerFile
            // 
            this.lnkOpenSchedulerFile.AutoSize = true;
            this.lnkOpenSchedulerFile.Location = new System.Drawing.Point(109, 4);
            this.lnkOpenSchedulerFile.Name = "lnkOpenSchedulerFile";
            this.lnkOpenSchedulerFile.Size = new System.Drawing.Size(191, 25);
            this.lnkOpenSchedulerFile.TabIndex = 8;
            this.lnkOpenSchedulerFile.TabStop = true;
            this.lnkOpenSchedulerFile.Text = "Open Scheduler File";
            this.lnkOpenSchedulerFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOpenSchedulerFile_LinkClicked);
            // 
            // colLastRun
            // 
            this.colLastRun.Text = "Last Run";
            // 
            // colInterval
            // 
            this.colInterval.Text = "Interval (min)";
            // 
            // colNextRun
            // 
            this.colNextRun.Text = "Scheduled For";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // AutomatedScreenshotsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lnkOpenSchedulerFile);
            this.Controls.Add(this.lnkRefresh);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.pnlProjectOptions);
            this.Controls.Add(this.lvScheduledProjects);
            this.Controls.Add(this.btnStartStop);
            this.Name = "AutomatedScreenshotsControl";
            this.Size = new System.Drawing.Size(1775, 1106);
            this.Load += new System.EventHandler(this.AutomatedScreenshotsControl_Load);
            this.pnlProjectOptions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.ListView lvScheduledProjects;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colPath;
        private System.Windows.Forms.ColumnHeader colDomains;
        private System.Windows.Forms.Button btnOpenProjectFolder;
        private System.Windows.Forms.GroupBox pnlProjectOptions;
        private System.Windows.Forms.Button btnProjectOptions;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.LinkLabel lnkRefresh;
        private System.Windows.Forms.LinkLabel lnkOpenSchedulerFile;
        private System.Windows.Forms.ColumnHeader colLastRun;
        private System.Windows.Forms.ColumnHeader colInterval;
        private System.Windows.Forms.ColumnHeader colNextRun;
        private System.Windows.Forms.Timer timer1;
    }
}
