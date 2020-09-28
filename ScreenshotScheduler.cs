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

        public string ConfigPath => this._schedulerStore.SchedulerFile;

        public List<FileProjectStore> GetScheduledProjectStores()
        {
            RefreshSettings();
            return this._settings is object
                ? this._settings.ScheduledProjects
                    .Select(p => this._projectStoreFactory.Create(p.ProjectId))
                    .Where(p => p.Exists)
                    .Cast<FileProjectStore>()
                    .ToList()
                : new List<FileProjectStore>();
        }

        public List<(ScheduledProject scheduledProject, FileProjectStore store)> GetScheduledProjects()
        {
            RefreshSettings();
            var scheduledProjects = this._settings?.ScheduledProjects ?? new List<ScheduledProject>();
            return scheduledProjects
                    .Select(p =>
                        (p,
                        store: (FileProjectStore)this._projectStoreFactory.Create(p.ProjectId)))
                    .Where(p => p.store.Exists)
                    .ToList();
        }

        //public IReadOnlyList<Project> ScheduledProjects => _scheduledProjects;
        private readonly IProjectStoreFactory _projectStoreFactory = new FileProjectStoreFactory();

        private readonly SchedulerStore _schedulerStore = new SchedulerStore();
        private SchedulerSettings _settings;

        public IProgress<TaskProgress> Progress { get; set; }

        public bool IsBusy => this._cancellableTask.Busy;

        // Stops any future projects from running and cancels any currently running ones.
        public bool Enabled
        {
            get => this._timer.Enabled;
            set
            {
                if (!value)
                {
                    this._cancellableTask?.Cancel();
                }
                this._timer.Enabled = value;
            }
        }

        private readonly CancellableTask _cancellableTask = new CancellableTask();

        private readonly System.Windows.Forms.Timer _timer =
            new System.Windows.Forms.Timer()
            {
                Interval = 5000,
            };

        public ScreenshotScheduler(Logger logger = null)
        {
            this.Logger = logger ?? new Logger(Logger.Default, new FileLogger("scheduler.log"));

            RefreshSettings();
            this._timer.Tick += Timer_Tick;
        }

        public void RefreshSettings()
        {
            this._settings = this._schedulerStore.Load();
        }

        public void ScheduleImmediateRun(string id)
        {
            this._settings.ScheduledProjects
                .Where(p => p.ProjectId == id)
                .ForEach(p => p.RunImmediately = true);
            this._schedulerStore.Save(this._settings);
        }

        private ScheduledProject NextScheduledProject()
        {
            bool IsScheduled(ScheduledProject p)
            {
                return p.Enabled
&& (
!p.LastRun.HasValue
|| p.LastRun.Value.Add(p.Interval) < DateTime.Now
);
            }

            return this._settings.ScheduledProjects.FirstOrDefault(p => p.RunImmediately)
                ?? this._settings.ScheduledProjects.FirstOrDefault(IsScheduled);
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (this.IsBusy)
            {
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();
            var nextScheduledProject = NextScheduledProject();

            if (nextScheduledProject is null)
            {
                return;
            }

            async Task TakeScreenshots(CancellationToken token)
            {
                await TryTakeScreenshotsAsync(nextScheduledProject, token);
            }

            await this._cancellableTask.PerformAsync(TakeScreenshots);
            nextScheduledProject.LastRun = DateTime.Now;
            nextScheduledProject.RunImmediately = false;
            this._schedulerStore.Save(this._settings);
            SessionCompleted?.Invoke(this, EventArgs.Empty);
            this.Logger.Log($"Project completed successfully in {sw.ElapsedMilliseconds}ms, {nextScheduledProject.ProjectId}");
        }

        private async Task<bool> TryTakeScreenshotsAsync(
            ScheduledProject scheduledProject,
            CancellationToken token)
        {
            try
            {
                string projectId = scheduledProject.ProjectId;
                var nextProjectStore = this._projectStoreFactory.Create(projectId);
                var project = nextProjectStore.Load();
                DeviceScreenshotter screenshotter = new DeviceScreenshotter(project);
                await screenshotter.TakeScreenshotsAsync(token, this.Progress);
                return true;
            }
            catch (TaskCanceledException)
            {
                this.Logger.Log("Scheduled screenshotting cancelled.", LogEntryType.Warning);
            }
            catch (Exception ex)
            {
                this.Logger.Log(new DiagnosticLogEntry(ex));
            }
            return false;
        }

        private IEnumerable<string> GetScheduledProjectPaths()
        {
            return Properties.Settings.Default?.AutomatedProjects?.Cast<string>()
?? Enumerable.Empty<string>();
        }

        private List<Project> LoadProjects(IEnumerable<string> paths)
        {
            return paths.Where(File.Exists)
.Select(LoadProject)
.ToList();
        }

        private Project LoadProject(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The project file wasn't found.", path);
            }

            FileProjectStore store = new FileProjectStore(path);
            return store.Load();
        }

        public void Dispose()
        {
            this._cancellableTask?.Dispose();
        }
    }
}