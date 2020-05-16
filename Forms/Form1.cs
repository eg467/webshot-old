using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Webshot
{
    public enum Device
    {
        Desktop, Mobile, Tablet,
    }

    public partial class Form1 : Form
    {
        public event EventHandler<ProjectFileEventArgs> CreateProject;

        public event EventHandler<ProjectFileEventArgs> LoadProject;

        public event EventHandler<SelectedUrisChangedEventArgs> SelectedUrisChanged;

        public event EventHandler ProjectNameChanged;

        public event EventHandler SaveProject;

        public event EventHandler SiteCrawlRequest;

        public event EventHandler ScreenshotRequest;

        public event EventHandler CancelTask;

        public event EventHandler ShowOptions;

        public event EventHandler ShowResults;

        public bool DisableUi
        {
            get => this.btnCancelTask.Enabled;
            set
            {
                this.btnCancelTask.Enabled = value;
                this.pnlCrawl.Enabled = !value;
                this.pnlSelectedPages.Enabled = !value;
                this.pnlProject.Enabled = !value;

                if (!value)
                {
                    this.progressBar.Value = 0;
                    this.progressBar.Maximum = 0;
                    this.lblStatus.Text = "";
                }
            }
        }

        public string ProjectName
        {
            get => this.txtProjectName.Text;
            set { this.txtProjectName.Text = value; }
        }

        public string ProjectPath
        {
            get => this.lnkProjectLocation.Text;
            set { this.lnkProjectLocation.Text = value; }
        }

        public IEnumerable<Uri> SpiderSeedUris
        {
            get => this.txtSpiderSeedUris.Text
                .Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(u => !u.Contains("://") ? $"https://{u}" : u)
                .Select(u => new Uri(u))
                .ToList();
            set
            {
                this.txtSpiderSeedUris.Text = string.Join(Environment.NewLine, value);
            }
        }

        public IEnumerable<Uri> SelectedUris =>
            ScreenshotUris.Where(x => x.Item2).Select(x => x.Item1);

        public IEnumerable<Tuple<Uri, bool>> ScreenshotUris
        {
            get => this.clbSelectedUris.Items.Cast<Uri>()
                    .Select((x, idx) => new Tuple<Uri, bool>(
                        x, this.clbSelectedUris.GetItemChecked(idx)));
            set
            {
                this.clbSelectedUris.Items.Clear();
                value.ForEach(x =>
                {
                    (Uri uri, bool enabled) = x;
                    this.clbSelectedUris.Items.Add(uri, enabled);
                });
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void HideThenFlashProjectSaved()
        {
            this.lblProjectSaved.Visible = false;
            void ShowDisplay()
            {
                FlashProjectSaved();
            }
            new Debouncer(ShowDisplay, 500, 1).Call();
        }

        public void FlashProjectSaved()
        {
            if (this.lblProjectSaved.Visible)
            {
                HideThenFlashProjectSaved();
            }

            this.lblProjectSaved.Visible = true;
            void HideDisplay()
            {
                this.lblProjectSaved.Visible = false;
            }
            new Debouncer(HideDisplay, 2000, 1).Call();
        }

        public void RefreshRecentProjects(IEnumerable<string> projectPaths)
        {
            var items = projectPaths
                .Select(p =>
                    new ToolStripButton(
                        p,
                        null,
                        (s, e) => OnLoadProject(new ProjectFileEventArgs(p)))
                    {
                        Size = new System.Drawing.Size(800, 30),
                        TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                        AutoSize = false
                    })
                .ToArray();

            this.btnRecentProjects.DropDownItems.AddRange(items);
        }

        protected void OnLoadProject(ProjectFileEventArgs args)
        {
            LoadProject?.Invoke(this, args);
        }

        protected void OnCreateProject(ProjectFileEventArgs args)
        {
            CreateProject?.Invoke(this, args);
        }

        protected void OnSaveProject()
        {
            SaveProject?.Invoke(this, EventArgs.Empty);
        }

        protected void OnSiteCrawlRequest()
        {
            SiteCrawlRequest?.Invoke(this, EventArgs.Empty);
        }

        protected void OnScreenshotRequest()
        {
            ScreenshotRequest?.Invoke(this, EventArgs.Empty);
        }

        protected void OnCancelTask()
        {
            CancelTask?.Invoke(this, EventArgs.Empty);
        }

        protected void OnSelectedUrisChanged(SelectedUrisChangedEventArgs args)
        {
            SelectedUrisChanged?.Invoke(this, args);
        }

        protected void OnProjectNameChanged()
        {
            ProjectNameChanged?.Invoke(this, EventArgs.Empty);
        }

        protected void OnShowOptions()
        {
            ShowOptions?.Invoke(this, EventArgs.Empty);
        }

        protected void OnShowResults()
        {
            ShowResults?.Invoke(this, EventArgs.Empty);
        }

        public string ChooseSaveFileDialog()
        {
            // User hasn't selected a directory/path yet
            return (DialogResult.OK == this.saveProjectFileDialog.ShowDialog())
                ? this.saveProjectFileDialog.FileName
                : null;
        }

        public void LoadProjectDirectory()
        {
            var lastDir = Properties.Settings.Default.OutputDir;
            var currentDir = Path.GetDirectoryName(typeof(Form1).Assembly.Location);
            var defaultDir = Path.Combine(currentDir, "Projects");
            this.openProjectFileDialog.InitialDirectory = lastDir ?? defaultDir;
            if (DialogResult.OK == this.openProjectFileDialog.ShowDialog())
            {
                string selectedPath = this.openProjectFileDialog.FileName;
                var args = new ProjectFileEventArgs(selectedPath);
                OnLoadProject(args);
            };
        }

        private void BtnStartCrawl_Click(object sender, EventArgs e)
        {
            OnSiteCrawlRequest();
        }

        public void UpdateProgress(ParsingProgress progress)
        {
            this.progressBar.Maximum = progress.Count;
            this.progressBar.Value = progress.CurrentIndex + 1;
            this.lblStatus.Text = progress.CurrentItem;
        }

        public void SelectPages()
        {
            //var frm = new ChoosePagesForm(.CrawlResults);
            //if (DialogResult.OK != frm.ShowDialog(this))
            //{
            //    return;
            //}
            //this.txtSelectedPages.Text =
            //    string.Join(Environment.NewLine, frm.IncludedPages);
        }

        private void BtnStartScreenshots_Click(object sender, EventArgs e)
        {
            OnScreenshotRequest();
        }

        private void BtnViewResults_Click(object sender, EventArgs e)
        {
            //Controller.ViewScreenshots();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            OnCancelTask();
        }

        public bool ConfirmOverwriteImages(int existingImages) =>
            DialogResult.Yes == MessageBox.Show(
                this,
                $@"This directory already contains {existingImages} files.
                Press 'Yes' if you want to delete and replace them.",
                "Overwrite Existing Images?",
                MessageBoxButtons.YesNo);

        private void BtnSaveProject_Click(object sender, EventArgs e)
        {
            OnSaveProject();
        }

        private void BtnCancelTask_Click(object sender, EventArgs e)
        {
            OnCancelTask();
        }

        private void BtnOpenProject_Click(object sender, EventArgs e)
        {
            this.openProjectFileDialog.Filter =
                $"Webshot Project|{FileProjectStore.ProjectFilename}";
            if (DialogResult.OK == this.openProjectFileDialog.ShowDialog())
            {
                var path = this.openProjectFileDialog.FileName;
                var args = new ProjectFileEventArgs(path);
                OnLoadProject(args);
            }
        }

        private void ShowProjectInExplorer()
        {
            System.Diagnostics.Process.Start(ProjectPath);
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnShowOptions();
        }

        private void LnkProjectLocation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowProjectInExplorer();
        }

        private void BtnViewProjectFolder_Click(object sender, EventArgs e)
        {
            ShowProjectInExplorer();
        }

        public string ChooseProjectFolder() =>
            (DialogResult.OK == this.folderOpenProject.ShowDialog())
            ? this.folderOpenProject.SelectedPath
            : null;

        private void TxtProjectName_TextChanged(object sender, EventArgs e)
        {
            OnProjectNameChanged();
        }

        private void btnShowResults_Click(object sender, EventArgs e)
        {
            OnShowResults();
        }
    }

    public class ProjectFileEventArgs : EventArgs
    {
        public string Path { get; set; }

        public ProjectFileEventArgs(string path)
        {
            this.Path = path;
        }
    }

    public class SelectedUrisChangedEventArgs : EventArgs
    {
        public IEnumerable<Uri> SelectedUris { get; }

        public SelectedUrisChangedEventArgs(IEnumerable<Uri> selectedUris)
        {
            SelectedUris = selectedUris;
        }
    }
}