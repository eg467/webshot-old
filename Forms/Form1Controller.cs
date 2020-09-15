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
        // Directory
        // Store
        // Project

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
                Utils.ChangeSettings(s => s.OutputDir = ProjectStore.ProjectDir);
                RefreshProjectUi();
            }
        }

        private FileProjectStore ProjectStore => Project?.Store as FileProjectStore;
        public string ProjectDirectory => ProjectStore.ProjectDir;
        private Options Options => Project?.Options;

        private readonly FormCreator<
           Form1,
           Form1Controller>
           _formCreator;

        public Form1 Form => _formCreator.Form;

        private CancellationTokenSource _cancellationTokenSource;

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
            if (!string.IsNullOrEmpty(projectDir))
            {
                LoadOrCreateProject(projectDir);
            }
        }

        private void ModifyProject(Action<Project> fn, bool saveImmediately = true)
        {
            fn(Project);
            DebouncedSave(saveImmediately);
        }

        private async Task ModifyProjectAsync(
            Func<Project, Task> fn,
            bool saveImmediately = true)
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

        private readonly CancellableTask _cancellableTask = new CancellableTask();

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
                await _cancellableTask.PerformAsync(async token =>
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
            await _cancellableTask.PerformAsync(async token =>
            {
                var stopwatch = Stopwatch.StartNew();
                ModifyProject(p => p.Input.SelectedUris = Form.SelectedUris.ToList());
                var ss = new DeviceScreenshotter(Project);
                await ss.TakeScreenshotsAsync(token, _progress);
                stopwatch.Stop();
                Debug.WriteLine($"Screenshots taken in {stopwatch.Elapsed.TotalSeconds} s");
            });
        }

        private void LoadOrCreateProject(string projectDirectory)
        {
            var store = new FileProjectStore(projectDirectory);
            Project = store.LoadProject();
        }

        private void SaveProject()
        {
            _debouncedProject.Save();
        }

        private void RefreshProjectUi()
        {
            // Wait for the Form Load event to refresh UI
            if (Form == null) return;

            _debouncedProject.Flush();
            Form.IsProjectLoaded = Project is object;
            Form.ProjectPath = ProjectDirectory;
            Form.ProjectName = Project.Name;
            Form.SpiderSeedUris = Project.Input.SpiderSeedUris;
            var selectedPages = Project.Input.SelectedUris;
            Form.ScreenshotUris = Project.CrawledPages.SitePages
                .Select(u => (u, selectedPages.Contains(u)));

            Form.BrokenLinks = Project.CrawledPages.BrokenLinks;
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
            Form.SiteCrawlRequest -= Form_SiteCrawlRequest;
            Form.ScreenshotRequest -= Form_ScreenshotRequest;
            Form.SelectedUrisChanged -= Form_SelectedUrisChanged;
            Form.ProjectNameChanged -= Form_ProjectNameChanged;
            Form.ShowOptions -= Form_ShowOptions;
            Form.ShowResults -= Form_ShowResults;
        }

        private void Form_Load(object sender, EventArgs e)
        {
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
            LoadOrCreateProject(e.Path);
        }

        private void Form_CreateProject(object sender, ProjectFileEventArgs e)
        {
            LoadOrCreateProject(e.Path);
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
            var controller = new OptionsFormController(Project);
            var form = controller.CreateForm();
            form.ShowDialog(Form);
        }

        private void Form_ShowResults(object sender, EventArgs e)
        {
            Dictionary<string, ScreenshotResults> screenshots = ProjectStore.GetScreenshots();
            var controller = new ViewResultsFormController(_debouncedProject, screenshots);
            var form = controller.CreateForm();
            form.ShowDialog(this.Form);
        }
    }
}