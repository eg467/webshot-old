using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Webshot
{
    internal class ScreenshotScheduler : IDisposable

    {
        private List<Project> _scheduledProjects = new List<Project>();

        private readonly IProgress<TaskProgress> _progress;

        public bool IsBusy => _cancellableTask.Busy;

        private readonly CancellableTask _cancellableTask = new CancellableTask();

        private readonly System.Windows.Forms.Timer _timer =
            new System.Windows.Forms.Timer()
            {
                Interval = 5000,
            };

        public ScreenshotScheduler(IProgress<TaskProgress> progress = null)
        {
            _timer.Tick += Timer_Tick;
            _scheduledProjects = LoadScheduledProjects();
            this._progress = progress;
        }

        private static readonly TimeSpan Timeout = TimeSpan.FromMinutes(5);

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (IsBusy) return;

            var nextProject = _scheduledProjects
                .FirstOrDefault(x => x.Options.SchedulerOptions.IsScheduled);

            if (nextProject is null) return;

            async Task Screenshoot(CancellationToken token) =>
                await TryScreenshootAsync(nextProject, token);

            await _cancellableTask.PerformAsync(Screenshoot);
        }

        private async Task<bool> TryScreenshootAsync(
            Project project,
            CancellationToken token)
        {
            try
            {
                var screenshotter = new DeviceScreenshotter(project);
                await screenshotter.TakeScreenshotsAsync(token, _progress);
                project.Options.SchedulerOptions.LastRun = DateTime.Now;
                project.Save();
                return true;
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Scheduled screenshotting cancelled.");
            }
            catch (Exception ex)
            {
                // TODO: Add logging
                Debug.WriteLine("Error completing scheduled screenshotting: " + ex.Message);
            }
            return false;
        }

        private List<Project> LoadScheduledProjects() =>
            Properties.Settings.Default.AutomatedProjects
                .Cast<string>()
                .Where(File.Exists)
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