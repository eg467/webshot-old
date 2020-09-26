using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Webshot.Forms;
using System.Drawing;
using Webshot.Store;
using System.IO;

namespace Webshot.Controls
{
    public partial class AutomatedScreenshotsControl : UserControl
    {
        public IProgress<TaskProgress> Progress
        {
            get => _screenshotScheduler.Progress;
            set => _screenshotScheduler.Progress = value;
        }

        private readonly Logger _logger = new Logger(Logger.Default, new FileLogger("scheduler.log"));
        private readonly ScreenshotScheduler _screenshotScheduler;

        private readonly StatsPageGenerator _statsPageGenerator =
            new StatsPageGenerator(Properties.Settings.Default.PerformanceFilePath);

        private FileProjectStore SelectedStore
        {
            get
            {
                if (this.lvScheduledProjects.SelectedItems.Count == 0) return null;
                var (_, store) = ((ScheduledProject, FileProjectStore))this.lvScheduledProjects.SelectedItems[0].Tag;
                return store;
            }
        }

        private void UpdateSelectedProjectUi()
        {
            var store = SelectedStore;
            this.pnlProjectOptions.Enabled = store is object;
            this.btnOpenProjectFolder.Text = store?.ProjectDir ?? "Open project directory";
        }

        public AutomatedScreenshotsControl()
        {
            InitializeComponent();
            _logger.Logged += Logger_Logged;
            _screenshotScheduler = new ScreenshotScheduler(_logger);
            _screenshotScheduler.SessionCompleted += ScreenshotScheduler_SessionCompleted;
        }

        private void ScreenshotScheduler_SessionCompleted(object sender, EventArgs e)
        {
            _statsPageGenerator.SavePage();
            var logMsg = $"Performance page saved to: {_statsPageGenerator.FilePath}";
            _logger.Log(logMsg);
        }

        private void Logger_Logged(object sender, LoggerEventArgs e)
        {
            string log = e.Entry.ToString();

            this.txtLog.Text = log + Environment.NewLine + this.txtLog.Text;
            this.txtLog.Select(0, log.Length);
            var (fg, bg) = GetColors();
            this.txtLog.SelectionColor = fg;
            this.txtLog.SelectionBackColor = bg;

            (Color fg, Color bg) GetColors()
            {
                switch (e.Entry.Type)
                {
                    case LogEntryType.Error:
                        return (Color.White, Color.Red);

                    case LogEntryType.Warning:
                        return (Color.DarkOrange, Color.Transparent);

                    default:
                        return (Color.Black, Color.Transparent);
                }
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            _screenshotScheduler.Enabled = !_screenshotScheduler.Enabled;
            this.btnStartStop.Text = _screenshotScheduler.Enabled ? "Stop" : "Start";
        }

        private void lvScheduledProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelectedProjectUi();
        }

        private void LoadProjects()
        {
            List<(ScheduledProject, FileProjectStore)> scheduled =
                _screenshotScheduler.GetScheduledProjects();

            this.lvScheduledProjects.Items.Clear();
            foreach ((ScheduledProject, FileProjectStore) s in scheduled)
            {
                var (scheduledProject, projectStore) = s;
                var project = projectStore.Load();

                var domains = project.Input.SelectedPages
                    .Select(u => u.Authority)
                    .Distinct();
                var domainLabel = string.Join(", ", domains);

                var subitems = new[] {
                    project.Name,
                    domainLabel,
                    projectStore.ProjectDir,
                    GetTimeLabel(scheduledProject.Enabled ? scheduledProject.LastRun : null),
                    scheduledProject.Interval.ToString(),
                    ""
                };

                var item = new ListViewItem(subitems)
                {
                    Tag = s
                };
                this.lvScheduledProjects.Items.Add(item);
            }
            RefreshTimeRemaining();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="time"></param>
        /// <returns>A time string if the datetime is for today, a full date/time string if for another day, or "Never" if null.</returns>
        private string GetTimeLabel(DateTime? time) =>
            time?.Date.Equals(DateTime.Now.Date) == true
                ? $"{time.Value.ToLongTimeString()} ({TimeDiffLabel(time.Value)})"
                : time.HasValue
                ? $"{time.Value.ToLongDateString()} ({TimeDiffLabel(time.Value)})"
                : "Never";

        private string TimeDiffLabel(DateTime date)
        {
            var diff = date.Subtract(DateTime.Now);

            if (Math.Abs(diff.TotalDays) > 1d)
            {
                return $"{diff.TotalDays:f2}D";
            }
            else if (Math.Abs(diff.TotalHours) > 1d)
            {
                return $"{diff.TotalHours:f2}H";
            }
            else
            {
                return $"{diff.TotalMinutes:f2}M";
            }
        }

        private void RefreshTimeRemaining()
        {
            var now = DateTime.Now;

            DateTime? NextScheduledTime(ScheduledProject s)
            {
                if (!s.Enabled) return null;
                if (!s.LastRun.HasValue || s.RunImmediately) return now;
                return s.LastRun.Value.Add(s.Interval);
            }

            foreach (ListViewItem item in this.lvScheduledProjects.Items)
            {
                var (sched, store) = ((ScheduledProject, FileProjectStore))item.Tag;
                var nextScheduledRun = NextScheduledTime(sched);
                item.SubItems[colLastRun.DisplayIndex].Text = GetTimeLabel(sched.LastRun);
                item.SubItems[colNextRun.DisplayIndex].Text = GetTimeLabel(nextScheduledRun);
            }
        }

        private void AutomatedScreenshotsControl_Load(object sender, EventArgs e)
        {
            LoadProjects();
        }

        private void btnOpenProjectFolder_Click(object sender, EventArgs e)
        {
            Process.Start(SelectedStore.ProjectDir);
        }

        private void btnProjectOptions_Click(object sender, EventArgs e)
        {
            var selectedIndex = this.lvScheduledProjects.SelectedIndices[0];
            var frm = new OptionsForm(SelectedStore.ProjectPath);
            frm.ShowDialog();
            LoadProjects();
            this.lvScheduledProjects.Items[selectedIndex].Selected = true;
        }

        private void lnkRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadProjects();
        }

        private void lnkOpenSchedulerFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(_screenshotScheduler.ConfigPath);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshTimeRemaining();
        }

        private void btnScheduleImmediately_Click(object sender, EventArgs e)
        {
            var selectedIndices = this.lvScheduledProjects.SelectedIndices;
            var selectedIndex = selectedIndices.Cast<int>().FirstOrDefault();
            _screenshotScheduler.ScheduleImmediateRun(SelectedStore.ProjectPath);
            LoadProjects();
            this.lvScheduledProjects.Items[selectedIndex].Selected = true;
        }

        private void btnChoosePerformanceStatsPath_Click(object sender, EventArgs e)
        {
            UpdatePerformancePagePath();
        }

        private void UpdatePerformancePagePath()
        {
            var current = Properties.Settings.Default.PerformanceFilePath;
            if (!string.IsNullOrEmpty(current))
            {
                var dir = Path.GetDirectoryName(current);
                this.savePerformanceFileDialog1.InitialDirectory = dir;
            }

            if (DialogResult.OK != this.savePerformanceFileDialog1.ShowDialog()) return;

            var path = this.savePerformanceFileDialog1.FileName;
            Properties.Settings.Default.PerformanceFilePath = path;
            Properties.Settings.Default.Save();
            _statsPageGenerator.FilePath = path;
            this.btnChoosePerformanceStatsPath.Text = $"Performance page path: {path}";
        }
    }
}