using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Webshot.Forms;
using System.Drawing;
using Webshot.Store;

namespace Webshot.Controls
{
    public partial class AutomatedScreenshotsControl : UserControl
    {
        public IProgress<TaskProgress> Progress
        {
            get => _screenshotScheduler.Progress;
            set => _screenshotScheduler.Progress = value;
        }

        private readonly ScreenshotScheduler _screenshotScheduler = new ScreenshotScheduler();

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
            _screenshotScheduler.Logger.Logged += Logger_Logged;
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

            if (diff.TotalDays > 1d)
            {
                return $"{diff.TotalDays:f2}D";
            }
            else if (diff.TotalHours > 1d)
            {
                return $"{diff.TotalHours:f2}H";
            }
            else
            {
                return $"{diff.Minutes}M";
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
    }
}