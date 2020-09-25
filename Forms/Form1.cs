using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Webshot.Forms;
using Webshot.Store;

namespace Webshot
{
    public enum Device
    {
        Desktop, Mobile, Tablet,
    }

    public partial class Form1 : Form
    {
        //public bool DisableUi
        //{
        //    get => this.btnCancelTask.Enabled;
        //    set
        //    {
        //        this.btnCancelTask.Enabled = value;
        //        this.pnlCrawl.Enabled = !value;
        //        this.pnlSelectedPages.Enabled = !value;
        //        this.pnlProjectSelection.Enabled = !value;

        //        if (!value)
        //        {
        //            this.progressBar.Value = 0;
        //            this.progressBar.Maximum = 0;
        //            this.lblStatus.Text = "";
        //        }
        //    }
        //}

        private readonly IProgress<TaskProgress> _progress;

        private DebouncedProjectSaver DebouncedProjectSaver =>
            this.projectControl.DebouncedProjectSaver;

        private Project Project
        {
            get => this.projectControl.Project;
            set
            {
                if (Project is object)
                {
                    Project.Store.Saved -= ProjectSaved;
                }
                this.projectControl.Project = value;
                if (value is object)
                {
                    value.Store.Saved += ProjectSaved;
                }
            }
        }

        private void ProjectSaved(object sender, ProjectSavedEventArgs e)
        {
            FlashProjectSaved();
        }

        private FileProjectStore ProjectStore => this.projectControl.ProjectStore;

        public Form1()
        {
            InitializeComponent();
            _progress = new Progress<TaskProgress>(UpdateProgress);
            this.projectControl.Progress = _progress;
            RefreshRecentProjects();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void HideThenFlashProjectSaved()
        {
            this.lblProjectSaved.Visible = false;
            new Debouncer(FlashProjectSaved, 500, 1).Call();
        }

        public void FlashProjectSaved()
        {
            if (this.lblProjectSaved.Visible)
            {
                HideThenFlashProjectSaved();
                return;
            }

            this.lblProjectSaved.Visible = true;
            void HideDisplay() => this.lblProjectSaved.Visible = false;
            new Debouncer(HideDisplay, 2000, 1).Call();
        }

        public void RefreshRecentProjects()
        {
            var recentProjects = Properties.Settings.Default.RecentProjects
                .Cast<string>()
                .Where(Directory.Exists)
                .Select(p =>
                    new ToolStripButton(
                        p,
                        null,
                        (_, __) => LoadOrCreateProject(p))
                    {
                        Size = new System.Drawing.Size(800, 30),
                        TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                        AutoSize = false
                    })
                .ToArray();

            this.btnRecentProjects.DropDownItems.AddRange(recentProjects);
        }

        public void LoadOrCreateProject(string projectDirectory)
        {
            var store = new FileProjectStore(projectDirectory);
            Project = store.Load();

            RefreshRecentProjects();
        }

        protected void SaveProject()
        {
            DebouncedProjectSaver.Save();
        }

        public string ChooseSaveFileDialog()
        {
            // User hasn't selected a directory/path yet
            return (DialogResult.OK == this.saveProjectFileDialog.ShowDialog())
                ? this.saveProjectFileDialog.FileName
                : null;
        }

        public string ChooseExistingProjectDirectory()
        {
            var lastDir = Properties.Settings.Default.OutputDir;
            var currentDir = Path.GetDirectoryName(typeof(Form1).Assembly.Location);
            var defaultDir = Path.Combine(currentDir, "Projects");
            this.openProjectFileDialog.InitialDirectory = lastDir ?? defaultDir;
            if (DialogResult.OK != this.openProjectFileDialog.ShowDialog()) return null;

            var projectFilePath = this.openProjectFileDialog.FileName;
            return Path.GetDirectoryName(projectFilePath);
        }

        public void LoadProjectDirectory()
        {
            var dir = ChooseExistingProjectDirectory();
            if (dir is null) return;
            LoadOrCreateProject(dir);
        }

        public void UpdateProgress(TaskProgress progress)
        {
            Invoke((Action)(() =>
            {
                this.progressBar.Maximum = progress.Count;
                this.progressBar.Value = progress.CurrentIndex;
                this.lblStatus.Text = progress.CurrentItem;
            }));
        }

        private void BtnSaveProject_Click(object sender, EventArgs e)
        {
            SaveProject();
        }

        private void BtnCancelTask_Click(object sender, EventArgs e)
        {
            this.projectControl.Cancel();
        }

        private void BtnOpenProject_Click(object sender, EventArgs e)
        {
            this.openProjectFileDialog.Filter =
                $"Webshot Project|{FileProjectStore.ProjectFilename}";
            if (DialogResult.OK != this.openProjectFileDialog.ShowDialog()) return;
            var projFilePath = this.openProjectFileDialog.FileName;
            var projectDir = Path.GetDirectoryName(projFilePath);
            LoadOrCreateProject(projectDir);
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new OptionsForm(ProjectStore.ProjectDir);
            form.ShowDialog();
        }

        private void BtnViewProjectFolder_Click(object sender, EventArgs e)
        {
            if (ProjectStore is null) return;
            Process.Start(ProjectStore.ProjectDir);
        }

        private void BtnShowResults_Click(object sender, EventArgs e)
        {
            if (ProjectStore is null) return;
            Dictionary<string, ScreenshotResults> screenshots = ProjectStore.GetAllResults();
            var controller = new ViewResultsFormController(DebouncedProjectSaver, screenshots);
            var form = controller.CreateForm();
            form.ShowDialog();
        }

        private void BtnQuickCreateProject_Click(object sender, EventArgs e)
        {
            var folder = FileProjectStore.CreateTempProjectDirectory(temporaryDir: false);
            LoadOrCreateProject(folder);
        }

        private void BtnCreateProject_Click(object sender, EventArgs e)
        {
            var dialogResult = this.folderOpenProject.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string folder = this.folderOpenProject.SelectedPath;
            if (!Directory.Exists(folder)) return;
            LoadOrCreateProject(folder);
        }

        private void btnRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }
    }
}