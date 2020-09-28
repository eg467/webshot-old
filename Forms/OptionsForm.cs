using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Webshot.Store;

namespace Webshot.Forms
{
    public partial class OptionsForm : Form
    {
        public event EventHandler Save;

        protected void OnSave()
        {
            Save?.Invoke(this, EventArgs.Empty);
        }

        private string _projectPath;
        private Project _project;
        private IProjectStoreFactory _projectStoreFactory = new FileProjectStoreFactory();

        public void SetProject(string projectPath)
        {
            if (!File.Exists(projectPath))
            {
                throw new FileNotFoundException("Project file doesn't exist.", projectPath);
            }
            _projectPath = projectPath;
            IProjectStore store = _projectStoreFactory.Create(_projectPath);
            _project = store.Load();
            ProjectNameUi = _project.Name;
            OptionsUi = _project.Options;
            ScheduledProjectUi = ScheduledProject;
        }

        private ScheduledProject ScheduledProject =>
            GetScheduledProject(new SchedulerStore().Load());

        private ScheduledProject GetScheduledProject(SchedulerSettings settings) =>
            settings.ScheduledProjects
            .FirstOrDefault(p => p.ProjectId == _projectPath);

        public string ProjectNameUi
        {
            get => this.txtProjectName.Text;
            set { this.txtProjectName.Text = value; }
        }

        /// <summary>
        /// The project options with settings in this form.
        /// (Some options will be set
        /// </summary>
        private Options OptionsUi
        {
            get => new Options
            {
                ScreenshotOptions = ScreenshotOptionsUi,
                ViewerOptions = ViewerOptionsUi,
                SpiderOptions = SpiderOptionsUi,
                Credentials = CredentialsUi,
            };
            set
            {
                ScreenshotOptionsUi = value.ScreenshotOptions;
                ViewerOptionsUi = value.ViewerOptions;
                SpiderOptionsUi = value.SpiderOptions;
                CredentialsUi = value.Credentials;
            }
        }

        private ScheduledProject ScheduledProjectUi
        {
            get
            {
                // Pull some values from current settings
                var originalValues = ScheduledProject;

                return this.cbScheduled.Checked
                    ? new ScheduledProject()
                    {
                        Enabled = this.cbScheduled.Checked,
                        Interval = TimeSpan.FromMinutes((double)this.numScheduleInterval.Value),
                        ProjectId = _projectPath,
                        RunImmediately = originalValues?.RunImmediately ?? false,
                        LastRun = originalValues?.LastRun
                    }
                    : null;
            }
            set
            {
                this.cbScheduled.Checked = value?.Enabled == true;
                var defaultInterval = new ScheduledProject().Interval;
                var interval = value?.Interval ?? defaultInterval;
                this.numScheduleInterval.Value = (int)interval.TotalMinutes;
            }
        }

        private ProjectCredentials CredentialsUi
        {
            get
            {
                var projectCreds = new ProjectCredentials();

                // Parsed in form 'domain:user:password'
                this.txtCreds.Text
                    .Split(
                        new[] { Environment.NewLine },
                        StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Split(':'))
                    .ForEach(c =>
                    {
                        if (c.Length != 3)
                        {
                            throw new ArgumentException(nameof(CredentialsUi));
                        }

                        var siteCreds = new AuthCredentials(c[1], c[2]);
                        projectCreds.CredentialsByDomain.Add(c[0], siteCreds);
                    });
                return projectCreds;
            }
            set
            {
                var creds =
                    value?.CredentialsByDomain
                    ?? new Dictionary<string, AuthCredentials>();

                var lines = creds.Select(c => $"{c.Key}:{c.Value.DecryptUser()}:{c.Value.DecryptPassword()}");
                this.txtCreds.Text = string.Join(Environment.NewLine, lines);
            }
        }

        private SpiderOptions SpiderOptionsUi
        {
            get => new SpiderOptions
            {
                UriBlacklistPattern =
                    this.txtSpiderBlacklist.Text.Length > 0
                        ? this.txtSpiderBlacklist.Text
                        : null,
                TrackExternalLinks = this.cbCrawlExternalLinks.Checked,
            };
            set
            {
                this.txtSpiderBlacklist.Text = value.UriBlacklistPattern;
                this.cbCrawlExternalLinks.Checked = value.TrackExternalLinks;
            }
        }

        private ViewerOptions ViewerOptionsUi
        {
            get => new ViewerOptions
            {
                ConstrainImageWidth = this.cbConstrainWidth.Checked,
                DeviceOutputScales = new Dictionary<Device, int>
                {
                    [Device.Desktop] = (int)this.numDesktopZoom.Value,
                    [Device.Tablet] = (int)this.numTabletZoom.Value,
                    [Device.Mobile] = (int)this.numMobileZoom.Value,
                }
            };
            set
            {
                this.cbConstrainWidth.Checked = value.ConstrainImageWidth;
                value.DeviceOutputScales = value.DeviceOutputScales ?? new ViewerOptions().DeviceOutputScales;
                this.numDesktopZoom.Value = value.DeviceOutputScales[Device.Desktop];
                this.numTabletZoom.Value = value.DeviceOutputScales[Device.Tablet];
                this.numMobileZoom.Value = value.DeviceOutputScales[Device.Mobile];
            }
        }

        private ScreenshotOptions ScreenshotOptionsUi
        {
            get => new ScreenshotOptions
            {
                StoreInTimestampedDir = this.cbStoreVersions.Checked,
                DeviceOptions = new Dictionary<Device, DeviceScreenshotOptions>()
                {
                    [Device.Desktop] = new DeviceScreenshotOptions(
                                Device.Desktop,
                                (int)this.numDesktopWidth.Value,
                                this.cbDesktop.Checked),
                    [Device.Tablet] = new DeviceScreenshotOptions(
                                Device.Tablet,
                                (int)this.numTabletWidth.Value,
                                this.cbTablet.Checked),
                    [Device.Mobile] = new DeviceScreenshotOptions(
                                Device.Mobile,
                                (int)this.numMobileWidth.Value,
                                this.cbMobile.Checked),
                },
            };
            set
            {
                this.cbStoreVersions.Checked = value.StoreInTimestampedDir;
                this.numDesktopWidth.Value = value.DeviceOptions[Device.Desktop].DisplayWidthInPixels;
                this.numTabletWidth.Value = value.DeviceOptions[Device.Tablet].DisplayWidthInPixels;
                this.numMobileWidth.Value = value.DeviceOptions[Device.Mobile].DisplayWidthInPixels;
                this.cbDesktop.Checked = value.DeviceOptions[Device.Desktop].Enabled;
                this.cbTablet.Checked = value.DeviceOptions[Device.Tablet].Enabled;
                this.cbMobile.Checked = value.DeviceOptions[Device.Mobile].Enabled;
            }
        }

        public OptionsForm()
        {
            InitializeComponent();
        }

        public OptionsForm(string path) : this()
        {
            SetProject(path);
        }

        private void SaveSettings()
        {
            if (_project is null) return;
            _project.Name = ProjectNameUi;
            _project.Options = OptionsUi;
            _project.Store.Save(_project);
            SaveSchedulerUiToSettings();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnResetOptions_Click(object sender, EventArgs e)
        {
            OptionsUi = new Options();
            this.cbScheduled.Checked = false;
            SaveSettings();
        }

        private void SaveSchedulerUiToSettings()
        {
            var store = new SchedulerStore();
            SchedulerSettings settings = store.Load();
            var scheduled = GetScheduledProject(settings);

            if (!this.cbScheduled.Checked)
            {
                if (scheduled is object)
                {
                    settings.ScheduledProjects.Remove(scheduled);
                    store.Save(settings);
                }
                return;
            }

            if (scheduled is null)
            {
                scheduled = new ScheduledProject();
                settings.ScheduledProjects.Add(scheduled);
            }

            scheduled.Enabled = true;
            decimal interval = this.numScheduleInterval.Value;
            scheduled.Interval = TimeSpan.FromMinutes((double)interval);
            scheduled.ProjectId = _projectPath;
            scheduled.RunImmediately = scheduled?.RunImmediately ?? false;
            scheduled.LastRun = scheduled?.LastRun;

            store.Save(settings);
        }
    }
}