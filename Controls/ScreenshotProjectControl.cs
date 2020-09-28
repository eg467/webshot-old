using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Webshot.Forms;

namespace Webshot.Controls
{
    public partial class ScreenshotProjectControl : UserControl
    {
        public event EventHandler BusyStateChanged;

        public DebouncedProjectSaver DebouncedProjectSaver { get; } = new DebouncedProjectSaver();

        private CancellableTask _cancellableTask = new CancellableTask();

        public IProgress<TaskProgress> Progress { get; set; }

        public Project Project
        {
            get => DebouncedProjectSaver.Project;
            set
            {
                if (DebouncedProjectSaver.Project == value) return;
                DebouncedProjectSaver.Project = value;
                RefreshUiFromProject();
            }
        }

        public FileProjectStore ProjectStore => (FileProjectStore)Project?.Store;
        public string ProjectDir => ProjectStore.ProjectDir;

        public ScreenshotProjectControl()
        {
            InitializeComponent();
        }

        public string ProjectNameUi
        {
            get => this.lnkProjectName.Text;
            set { this.lnkProjectName.Text = value; }
        }

        public string ProjectPathUi
        {
            get => this.lnkProjectLocation.Text;
            set { this.lnkProjectLocation.Text = value; }
        }

        public IEnumerable<Uri> SpiderSeedUrisUi
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<BrokenLink> BrokenLinks
        {
            get => this.btnBrokenLinks.Tag as List<BrokenLink>;
            set
            {
                var brokenLinks = value ?? new List<BrokenLink>();
                this.btnBrokenLinks.Tag = brokenLinks;
                this.btnBrokenLinks.Visible = brokenLinks.Any();
            }
        }

        public IEnumerable<Uri> SelectedUris =>
            SiteUrisUi.Where(x => x.selected).Select(x => x.uri);

        public IEnumerable<(Uri uri, bool selected)> SiteUrisUi
        {
            get => this.clbSelectedUris.Items.Cast<Uri>()
                    .Select((x, idx) => (x, this.clbSelectedUris.GetItemChecked(idx)));
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

        public void RefreshUiFromProject()
        {
            Visible = Project is object;
            SetFollowLinks(SpiderOptions.FollowLinks);
            SetTrackExternalDomains(SpiderOptions.TrackExternalLinks);
            SpiderSeedUrisUi = Project.Input.SpiderSeedUris;
            ProjectNameUi = Project.Name;
            ProjectPathUi = ProjectDir;
            BrokenLinks = Project.CrawledPages.BrokenLinks;
            SiteUrisUi = Project.Input.SiteUris;
        }

        private async void btnTakeScreenshots_Click(object sender, EventArgs e)
        {
            try
            {
                Busy = true;
                await _cancellableTask.PerformAsync(async token =>
                {
                    var stopwatch = Stopwatch.StartNew();
                    Project.Input.SiteUris = SiteUrisUi.ToList();
                    var ss = new DeviceScreenshotter(Project);
                    await ss.TakeScreenshotsAsync(token, Progress);
                    stopwatch.Stop();
                    Debug.WriteLine($"Screenshots taken in {stopwatch.Elapsed.TotalSeconds} s");
                });
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("The task has been canceled.");
            }
            finally
            {
                Busy = false;
                RefreshUiFromProject();
            }
        }

        private SpiderOptions SpiderOptions => Project.Options.SpiderOptions;

        private void SetFollowLinks(bool value)
        {
            if (SpiderOptions.FollowLinks != value)
            {
                SpiderOptions.FollowLinks = value;
                DebouncedProjectSaver.Save();
            }
            this.cbSpiderFollowLinks.Checked = value;
        }

        private void SetTrackExternalDomains(bool value)
        {
            if (SpiderOptions.TrackExternalLinks != value)
            {
                SpiderOptions.TrackExternalLinks = value;
                DebouncedProjectSaver.Save();
            }
            this.cbSpiderIncludeExternalDomains.Checked = value;
        }

        private async void btnCrawlPages_Click(object sender, EventArgs e)
        {
            var seedUris = SpiderSeedUrisUi.ToList();
            SetFollowLinks(this.cbSpiderFollowLinks.Checked);
            SetTrackExternalDomains(this.cbSpiderIncludeExternalDomains.Checked);
            Project.Input.SpiderSeedUris = seedUris;
            DebouncedProjectSaver.SaveNow();

            var projectOptions = Project.Options;
            var spiderOptions = projectOptions.SpiderOptions;
            var creds = projectOptions.Credentials;
            if (seedUris?.Any() != true)
            {
                throw new InvalidOperationException("You must provide seed URIs to crawl.");
            }

            Busy = true;
            await _cancellableTask.PerformAsync(async token =>
            {
                var spider = new Spider(seedUris, spiderOptions, creds);

                try
                {
                    var crawlResults = await spider.Crawl(token, Progress);
                    SetCrawlResults(crawlResults);
                }
                catch (TaskCanceledException)
                {
                    MessageBox.Show("The operation has been cancelled.");
                }
            });
            Busy = false;
        }

        private void SetCrawlResults(CrawlResults results)
        {
            Project.CrawledPages = results;
            // Select all results by default.
            Project.Input.SiteUris = results.SitePages
                .Select(x => (x, true))
                .ToList();
            Project.Save();

            SiteUrisUi = Project.Input.SiteUris;
        }

        /// <summary>
        /// Use <see cref="Busy"/> instead to ensure event fire.
        /// </summary>
        private bool _busy;

        public bool Busy
        {
            get => _busy;
            set
            {
                if (value == _busy) return;
                _busy = value;
                Enabled = !value;
                OnBusyStateChanged();
            }
        }

        protected void OnBusyStateChanged()
        {
            BusyStateChanged?.Invoke(this, EventArgs.Empty);
        }

        private void clbSelectedUris_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            DebouncedProjectSaver.Project.Input.SiteUris = SiteUrisUi.ToList();
            var uris = DebouncedProjectSaver.Project.Input.SiteUris;
            uris[e.Index] = (uris[e.Index].uri, e.NewValue == CheckState.Checked);
            DebouncedProjectSaver.Save();
        }

        private void lnkProjectLocation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(ProjectStore.ProjectDir);
        }

        public void Cancel()
        {
            _cancellableTask.Cancel();
        }

        private void btnBrokenLinks_Click(object sender, EventArgs e)
        {
            var frm = new BrokenLinksForm();
            frm.UpdateLinks(Project.CrawledPages.BrokenLinks);
            frm.ShowDialog();
        }
    }
}