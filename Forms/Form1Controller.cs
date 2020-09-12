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
                    Project = !string.IsNullOrEmpty(value)
                    ? FileProjectStore.CreateOrLoadProject(value)
                    : null;
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
        /// The current project after all pending changes have been saved.
        /// </summary>
        private Project FlushedProject => _debouncedProject.FlushedProject;

        /// <summary>
        /// The current project that may have unsaved changes.
        /// </summary>
        public Project Project
        {
            get => _debouncedProject.Project;
            private set
            {
                _debouncedProject.Project = value;
                if (ProjectStore?.IsSaved == false)
                {
                    _debouncedProject.Save(true);
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

        private void CancelTask()
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource = null;
        }

        private void TemporarilyDisableUi(Action action)
        {
            Form.DisableUi = true;
            action();
            Form.DisableUi = false;
        }

        private async Task TemporarilyDisableUiAsync(Func<Task> action)
        {
            Form.DisableUi = true;
            await action();
            Form.DisableUi = false;
        }

        private bool TaskInProgress => Form.DisableUi;

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
            _debouncedProject.Save(saveImmediately);
        }

        private async Task ModifyProjectAsync(Func<Project, Task> fn, bool saveImmediately = true)
        {
            await fn(Project);
            _debouncedProject.Save(saveImmediately);
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
                             p.Output.CrawlResults = await spider.Crawl(token, _progress);

                             // Select all results by default.
                             p.Input.SelectedUris =
                                  Project.Output.CrawlResults.SitePages.ToList();
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

                var ssOptions = Options.ScreenshotOptions;
                var createdAt = DateTime.Now;
                (string relImgDir, string absImgDir) = GetImageDir();
                CreateImageDir(absImgDir);
                var results = new ScreenshotResults
                {
                    Timestamp = createdAt,
                };

                using (var ss = new Screenshotter(
                    ssOptions,
                    Project.Output.CrawlResults?.BrokenLinks,
                    Options.Credentials))
                {
                    int i = 0;
                    var count = Project.Input.SelectedUris.Count();
                    foreach (var uri in Project.Input.SelectedUris)
                    {
                        try
                        {
                            if (token.IsCancellationRequested)
                            {
                                MessageBox.Show("The screenshotting task was canceled.");
                                return;
                            }

                            _progress.Report(new TaskProgress
                            {
                                Count = count,
                                CurrentIndex = ++i,
                                CurrentItem = uri.AbsoluteUri
                            });
                            var result = new DeviceScreenshots(uri);
                            await ScreenshotPageAsAllDevices(ss, uri, result);
                            results.Screenshots.Add(result);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }

                ModifyProject(p => p.Output.Screenshots = results);

                stopwatch.Stop();
                Debug.WriteLine($"Screenshots taken in {stopwatch.Elapsed.TotalSeconds} s");

                // LOCAL FUNCTIONS

                // Returns <relative, absolute> directory paths
                Tuple<string, string> GetImageDir()
                {
                    var basePath = Path.Combine(ProjectStore.ProjectDir, "Images");

                    string absPath = ssOptions.StoreInTimestampedDir
                        ? Utils.CreateTimestampDirectory(basePath, createdAt)
                        : basePath;

                    string relPath = absPath.Replace(ProjectStore.ProjectDir, "");

                    return new Tuple<string, string>(relPath, absPath);
                }

                void CreateImageDir(string dir)
                {
                    if (Directory.Exists(dir))
                    {
                        var numExistingFiles = Directory.GetFiles(dir).Count();
                        if (!Form.ConfirmOverwriteImages(numExistingFiles))
                        {
                            return;
                        }
                        Directory.Delete(dir, true);
                    }
                    Directory.CreateDirectory(dir);
                }

                async Task ScreenshotPageAsAllDevices(
                    Screenshotter ss,
                    Uri url,
                    DeviceScreenshots result)
                {
                    var sizes = Options.ScreenshotOptions.DeviceOptions
                        .Where(x => x.Value.Enabled && x.Value.DisplayWidthInPixels > 0)
                        .Select(x =>
                            new KeyValuePair<Device, int>(
                                x.Key,
                                x.Value.DisplayWidthInPixels));

                    foreach (var size in sizes)
                    {
                        await ScreenshotPageAsDeviceAsync(ss, url, result, size);
                    }
                }

                async Task ScreenshotPageAsDeviceAsync(
                    Screenshotter ss,
                    Uri url,
                    DeviceScreenshots result,
                    KeyValuePair<Device, int> size)
                {
                    var device = size.Key;
                    var width = size.Value;
                    var baseName = Screenshotter.SanitizeFilename(url.ToString());
                    var filename = $"{baseName}.{device}{Screenshotter.ImageExtension}";
                    var relPath = Path.Combine(relImgDir, filename);
                    var absPath = Path.Combine(absImgDir, filename);

                    try
                    {
                        await Task.Run(
                            () => ss.TakeScreenshot(url.ToString(), absPath, width));
                        result.Paths.Add(device, relPath);
                    }
                    catch (Exception ex)
                    {
                        result.Error += ex.Message + Environment.NewLine;
                    }
                }
            });
        }

        private CrawlResults CrawlResults => Project.Output.CrawlResults;

        private IReadOnlyDictionary<Device, DeviceScreenshotOptions> GetDeviceOptions() =>
            Options.ScreenshotOptions.DeviceOptions;

        private void LoadProject(string path = null)
        {
            ProjectDirectory = path
                ?? ProjectStore?.ProjectDir
                ?? Form?.ChooseProjectFolder();
        }

        private void SaveProject(string path = null)
        {
            ProjectStore.ProjectDir = path
                ?? ProjectStore.ProjectDir
                ?? Form.ChooseSaveFileDialog()
                ?? FileProjectStore.CreateProjectDirectory(temporary: true);

            ProjectStore.Save(Project);
        }

        private bool OnDisk => Directory.Exists(ProjectStore.ProjectDir);

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
            Form.ScreenshotUris = Project.Output.CrawlResults.SitePages
                .Select(u => new Tuple<Uri, bool>(
                    u,
                    Project.Input.SelectedUris.Contains(u)));

            Form.BrokenLinks = Project.Output.CrawlResults.BrokenLinks;
        }

        private void ViewScreenshots()
        {
            var controller = new ViewResultsFormController(_debouncedProject);
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
                ?? FileProjectStore.CreateProjectDirectory(temporary: false);
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