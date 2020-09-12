using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Webshot.Forms
{
    public partial class OptionsForm : Form
    {
        public event EventHandler Save;

        protected void OnSave()
        {
            Save?.Invoke(this, EventArgs.Empty);
        }

        public string ProjectName
        {
            get => this.txtProjectName.Text;
            set { this.txtProjectName.Text = value; }
        }

        /// <summary>
        /// The project options with settings in this form.
        /// (Some options will be set
        /// </summary>
        public Options Options
        {
            get => new Options
            {
                ScreenshotOptions = ScreenshotOptions,
                ViewerOptions = ViewerOptions,
                SpiderOptions = SpiderOptions,
                Credentials = Credentials,
            };
            set
            {
                ScreenshotOptions = value.ScreenshotOptions;
                ViewerOptions = value.ViewerOptions;
                SpiderOptions = value.SpiderOptions;
                Credentials = value.Credentials;
            }
        }

        public ProjectCredentials Credentials
        {
            get
            {
                var projectCreds = new ProjectCredentials();

                this.txtCreds.Text
                    .Split(
                        new[] { Environment.NewLine },
                        StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Split(':'))
                    .ForEach(c =>
                    {
                        if (c.Length != 3)
                        {
                            throw new ArgumentException(nameof(Credentials));
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

                var lines = creds.Select(c => $"{c.Key}:{c.Value.User}:{c.Value.Password}");
                this.txtCreds.Text = string.Join(Environment.NewLine, lines);
            }
        }

        public SpiderOptions SpiderOptions
        {
            get => new SpiderOptions
            {
                UriBlacklistPattern =
                    this.txtSpiderBlacklist.Text.Length > 0
                        ? this.txtSpiderBlacklist.Text
                        : null,
                FollowExternalLinks = this.cbCrawlExternalLinks.Checked,
            };
            set
            {
                this.txtSpiderBlacklist.Text = value.UriBlacklistPattern;
                this.cbCrawlExternalLinks.Checked = value.FollowExternalLinks;
            }
        }

        public ViewerOptions ViewerOptions
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

        public ScreenshotOptions ScreenshotOptions
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            OnSave();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
        }

        private void BtnResetOptions_Click(object sender, EventArgs e)
        {
            Options = new Options();
        }
    }
}