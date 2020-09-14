using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Webshot.Forms;

namespace Webshot
{
    public class Form1Controller : IFormController<Form1>
    {
        private FileProjectStore ProjectStore
        {
            get => Project?.Store as FileProjectStore;
            set { Project = value != null ? new Project(value) : null; }
        }

        private string ProjectDirectory
        {
            get => ProjectStore?.ProjectDir;
            set
            {
                try
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        Project = null;
                        return;
                    }

                    var store = new FileProjectStore(value);
                    Project = store.ProjectExists ? store.Load() : store.CreateProject();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error loading your project: " + ex.Message);
                    throw;
                }
            }
        }

        private readonly DebouncedProject _debouncedProject = new DebouncedProject();

        /// <summary>
        /// The current project that may have unsaved changes.
        /// </summary>
        public Project Project
        {
            get => _debouncedProject.Project;
            private set
            {
                _debouncedProject.Project = value;
                if (ProjectStore?.ProjectExists == false)
                {
                    _debouncedProject.SaveNow();
                }
                Utils.ChangeSettings(s => s.OutputDir = ProjectStore.ProjectDir);
                RefreshProjectUi();
            }
        }

        private Options Options => Project?.Options;

        private readonly FormCreator<
           Form1,
           Form1Controller>
           _formCreator;

        public Form1 Form => _formCreator.Form;

        private CancellationTokenSource _cancellationTokenSource;
        private bool IsBusy => !(CancellationTokenSource is null);

        private CancellationTokenSource CancellationTokenSource
        {
            get => _cancellationTokenSource;
            set
            {
                if (value != null && _cancellationTokenSource != null)
                {
                    throw new InvalidOperationException(
                        "The cancellation token is already set." +
                        "Set it to null before using another.");
                }
                _cancellationTokenSource = value;
            }
        }

        private static IEnumerable<string> GetRecentProjects() =>
            Properties.Settings.Default.RecentProjects
                .Cast<string>()
                .Where(Directory.Exists);

        private async Task TemporarilyDisableUiAsync(Func<Task> action)
        {
            Form.DisableUi = true;
            await action();
            Form.DisableUi = false;
        }

        public Form1Controller(string projectDir = null)
        {
            _progress = new Progress<TaskProgress>(p =>
            {
                Form?.UpdateProgress(p);
            });
            _formCreator = new FormCreator<Form1, Form1Controller>(this);
            _debouncedProject.Saved += (s, e) => Form.FlashProjectSaved();
            ProjectDirectory = projectDir;
        }

        private void ModifyProject(Action<Project> fn, bool saveImmediately = true)
        {
            fn(Project);
            DebouncedSave(saveImmediately);
        }

        private async Task ModifyProjectAsync(Func<Project, Task> fn, bool saveImmediately = true)
        {
            await fn(Project);
            DebouncedSave(saveImmediately);
        }

        private void DebouncedSave(bool saveImmediately)
        {
            if (saveImmediately)
            {
                _debouncedProject.SaveNow();
            }
            else
            {
                _debouncedProject.Save();
            }
        }

        /// <summary>
        /// Performs an action that can be cancelled.
        /// </summary>
        /// <remarks>Not thread safe.</remarks>
        /// <param name="fn"></param>
        private void CancellableAction(Action<CancellationToken> fn)
        {
            using (CancellationTokenSource = new CancellationTokenSource())
            {
                fn(CancellationTokenSource.Token);
            }
            CancellationTokenSource = null;
        }

        /// <summary>
        /// Performs an action that can be cancelled.
        /// </summary>
        /// <remarks>Not thread safe.</remarks>
        /// <param name="fn"></param>
        private async Task CancellableActionAsync(Func<CancellationToken, Task> fn)
        {
            using (CancellationTokenSource = new CancellationTokenSource())
            {
                await fn(CancellationTokenSource.Token);
            }
            CancellationTokenSource = null;
        }

        public IProgress<TaskProgress> _progress;

        /// <summary>
        ///
        /// </summary>
        /// <exception cref="OperationCanceledException"></exception>
        private async Task Crawl()
        {
            ModifyProject(p => p.Input.SpiderSeedUris = Form.SpiderSeedUris.ToList());

            var spiderSeedUris = Project.Input.SpiderSeedUris;
            var spiderOptions = Options.SpiderOptions;
            var creds = Options.Credentials;
            var initialUris = Project.Input.SpiderSeedUris;
            if (initialUris?.Any() != true)
            {
                throw new InvalidOperationException("You must provide seed URIs to crawl.");
            }

            await TemporarilyDisableUiAsync(async () =>
                await CancellableActionAsync(async token =>
                 {
                     var spider = new Spider(initialUris, spiderOptions, creds);

                     try
                     {
                         await ModifyProjectAsync(async p =>
                         {
                             p.CrawledPages = await spider.Crawl(token, _progress);

                             // Select all results by default.
                             p.Input.SelectedUris = p.CrawledPages.SitePages.ToList();
                         });
                     }
                     catch (OperationCanceledException)
                     {
                         MessageBox.Show("The operation has been cancelled.");
                     }
                 })
            );

            RefreshProjectUi();
        }

        private async Task TakeScreenshots()
        {
            await CancellableActionAsync(async token =>
            {
                var stopwatch = Stopwatch.StartNew();
                ModifyProject(p => p.Input.SelectedUris = Form.SelectedUris.ToList());
                var ss = new DeviceScreenshotter(Project);
                await ss.TakeScreenshotsAsync(token, _progress);
                stopwatch.Stop();
                Debug.WriteLine($"Screenshots taken in {stopwatch.Elapsed.TotalSeconds} s");
            });
        }

        private void LoadProject(string path = null)
        {
            ProjectDirectory = path
                ?? ProjectStore?.ProjectDir
                ?? Form?.ChooseProjectFolder();
        }

        /// <summary>
        /// Try to save the project to one of the following, in order:
        /// The store's location, a user-selected directory, a temporary directory.
        /// </summary>
        /// <param name="path"></param>
        private void SaveProject(string path = null)
        {
            ProjectStore.ProjectDir = path
                ?? ProjectStore.ProjectDir
                ?? Form.ChooseSaveFileDialog()
                ?? FileProjectStore.CreateTempProjectDirectory(temporaryDir: true);

            ProjectStore.Save(Project);
        }

        private void RefreshProjectUi()
        {
            if (Form == null)
            {
                // Wait for the Form Load event to refresh UI
                return;
            }
            Form.ProjectPath = ProjectDirectory;
            Form.ProjectName = Project.Name;
            Form.SpiderSeedUris = Project.Input.SpiderSeedUris;
            Form.ScreenshotUris = Project.CrawledPages.SitePages
                .Select(u => new Tuple<Uri, bool>(
                    u,
                    Project.Input.SelectedUris.Contains(u)));

            Form.BrokenLinks = Project.CrawledPages.BrokenLinks;
        }

        private void ViewScreenshots()
        {
            Dictionary<string, ScreenshotResults> screenshots = ProjectStore.GetScreenshots();
            var controller = new ViewResultsFormController(_debouncedProject, screenshots);
            var form = controller.CreateForm();

            form.ShowDialog(this.Form);
        }

        private void ViewOptions()
        {
            var controller = new OptionsFormController(Project);
            var form = controller.CreateForm();
            form.ShowDialog(Form);
        }

        public Form1 CreateForm()
        {
            UnwireFormEvents();
            var form = _formCreator.CreateForm();
            WireFormEvents();
            return form;
        }

        private void WireFormEvents()
        {
            if (Form == null) return;

            Form.Load += Form_Load;
            Form.CreateProject += Form_CreateProject;
            Form.LoadProject += Form_LoadProject;
            Form.SaveProject += Form_SaveProject;
            Form.CreateProject += Form_CreateProject;
            Form.SiteCrawlRequest += Form_SiteCrawlRequest;
            Form.ScreenshotRequest += Form_ScreenshotRequest;
            Form.SelectedUrisChanged += Form_SelectedUrisChanged;
            Form.ProjectNameChanged += Form_ProjectNameChanged;
            Form.ShowOptions += Form_ShowOptions;
            Form.ShowResults += Form_ShowResults;
        }

        private void UnwireFormEvents()
        {
            if (Form == null) return;

            Form.Load -= Form_Load;
            Form.CreateProject -= Form_CreateProject;
            Form.LoadProject -= Form_LoadProject;
            Form.SaveProject -= Form_SaveProject;
            Form.CreateProject -= Form_CreateProject;
            Form.SiteCrawlRequest -= Form_SiteCrawlRequest;
            Form.ScreenshotRequest -= Form_ScreenshotRequest;
            Form.SelectedUrisChanged -= Form_SelectedUrisChanged;
            Form.ProjectNameChanged -= Form_ProjectNameChanged;
            Form.ShowOptions -= Form_ShowOptions;
            Form.ShowResults -= Form_ShowResults;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            LoadProject();
            Form.RefreshRecentProjects(GetRecentProjects());
        }

        private async void Form_ScreenshotRequest(object sender, EventArgs e)
        {
            await TakeScreenshots();
        }

        private async void Form_SiteCrawlRequest(object sender, EventArgs e)
        {
            await Crawl();
        }

        private void Form_SaveProject(object sender, EventArgs e)
        {
            SaveProject();
        }

        private void Form_LoadProject(object sender, ProjectFileEventArgs e)
        {
            LoadProject(e.Path);
        }

        private void Form_CreateProject(object sender, ProjectFileEventArgs e)
        {
            ProjectDirectory =
                e.Path
                ?? FileProjectStore.CreateTempProjectDirectory(temporaryDir: false);
        }

        private void Form_SelectedUrisChanged(object sender, SelectedUrisChangedEventArgs e)
        {
            ModifyProject(p => p.Input.SelectedUris = e.SelectedUris.ToList(), false);
        }

        private void Form_ProjectNameChanged(object sender, EventArgs e)
        {
            ModifyProject(p => p.Name = Form.ProjectName, false);
        }

        private void Form_ShowOptions(object sender, EventArgs e)
        {
            ViewOptions();
        }

        private void Form_ShowResults(object sender, EventArgs e)
        {
            ViewScreenshots();
        }
    }
}