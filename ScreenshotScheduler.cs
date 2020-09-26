using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Webshot.Store;

namespace Webshot
{
    internal class ScreenshotScheduler : IDisposable
    {
        public event EventHandler SessionCompleted;

        public Logger Logger { get; }

        public string ConfigPath => _schedulerStore.SchedulerFile;

        public List<FileProjectStore> GetScheduledProjectStores()
        {
            RefreshSettings();
            return _settings is object
                ? _settings.ScheduledProjects
                    .Select(p => _projectStoreFactory.Create(p.ProjectId))
                    .Where(p => p.Exists)
                    .Cast<FileProjectStore>()
                    .ToList()
                : new List<FileProjectStore>();
        }

        public List<(ScheduledProject scheduledProject, FileProjectStore store)> GetScheduledProjects()
        {
            RefreshSettings();
            var scheduledProjects = _settings?.ScheduledProjects ?? new List<ScheduledProject>();
            return scheduledProjects
                    .Select(p =>
                        (p,
                        store: (FileProjectStore)_projectStoreFactory.Create(p.ProjectId)))
                    .Where(p => p.store.Exists)
                    .ToList();
        }

        //public IReadOnlyList<Project> ScheduledProjects => _scheduledProjects;
        private readonly IProjectStoreFactory _projectStoreFactory = new FileProjectStoreFactory();

        private readonly SchedulerStore _schedulerStore = new SchedulerStore();
        private SchedulerSettings _settings;

        public IProgress<TaskProgress> Progress { get; set; }

        public bool IsBusy => _cancellableTask.Busy;

        public bool Enabled
        {
            get => _timer.Enabled;
            set => _timer.Enabled = value;
        }

        private readonly CancellableTask _cancellableTask = new CancellableTask();

        private readonly System.Windows.Forms.Timer _timer =
            new System.Windows.Forms.Timer()
            {
                Interval = 5000,
            };

        public ScreenshotScheduler(Logger logger = null)
        {
            Logger = logger ?? new Logger(Logger.Default, new FileLogger("scheduler.log"));

            RefreshSettings();
            _timer.Tick += Timer_Tick;
        }

        public void RefreshSettings()
        {
            _settings = _schedulerStore.Load();
        }

        public void ScheduleImmediateRun(string id)
        {
            _settings.ScheduledProjects
                .Where(p => p.ProjectId == id)
                .ForEach(p => p.RunImmediately = true);
            _schedulerStore.Save(_settings);
        }

        private ScheduledProject NextScheduledProject()
        {
            bool IsScheduled(ScheduledProject p) =>
                p.Enabled
                && (
                    !p.LastRun.HasValue
                    || p.LastRun.Value.Add(p.Interval) < DateTime.Now
                );

            return _settings.ScheduledProjects.FirstOrDefault(p => p.RunImmediately)
                ?? _settings.ScheduledProjects.FirstOrDefault(IsScheduled);
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (IsBusy) return;
            var sw = Stopwatch.StartNew();
            ScheduledProject nextScheduledProject = NextScheduledProject();

            if (nextScheduledProject is null) return;

            async Task TakeScreenshots(CancellationToken token) =>
                await TryTakeScreenshotsAsync(nextScheduledProject, token);

            await _cancellableTask.PerformAsync(TakeScreenshots);
            nextScheduledProject.LastRun = DateTime.Now;
            nextScheduledProject.RunImmediately = false;
            _schedulerStore.Save(_settings);
            SessionCompleted?.Invoke(this, EventArgs.Empty);
            Logger.Log($"Project completed successfully in {sw.ElapsedMilliseconds}ms, {nextScheduledProject.ProjectId}");
        }

        private async Task<bool> TryTakeScreenshotsAsync(
            ScheduledProject scheduledProject,
            CancellationToken token)
        {
            try
            {
                string projectId = scheduledProject.ProjectId;
                IProjectStore nextProjectStore = _projectStoreFactory.Create(projectId);
                Project project = nextProjectStore.Load();
                var screenshotter = new DeviceScreenshotter(project);
                await screenshotter.TakeScreenshotsAsync(token, Progress);
                return true;
            }
            catch (TaskCanceledException)
            {
                Logger.Log("Scheduled screenshotting cancelled.", LogEntryType.Warning);
            }
            catch (Exception ex)
            {
                Logger.Log("Error completing scheduled screenshotting: " + ex.Message, LogEntryType.Error);
            }
            return false;
        }

        private IEnumerable<string> GetScheduledProjectPaths() =>
            Properties.Settings.Default?.AutomatedProjects?.Cast<string>()
            ?? Enumerable.Empty<string>();

        private List<Project> LoadProjects(IEnumerable<string> paths) =>
            paths.Where(File.Exists)
                .Select(LoadProject)
                .ToList();

        private Project LoadProject(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The project file wasn't found.", path);
            }

            var store = new FileProjectStore(path);
            return store.Load();
        }

        public void Dispose()
        {
            _cancellableTask?.Dispose();
        }
    }
}