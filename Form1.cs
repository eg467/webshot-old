using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Webshot.Spider;

namespace Webshot
{
    public enum Device
    {
        Desktop, Mobile, Tablet,
    }

    public partial class Form1 : Form
    {
        private const string ManifestFilename = "manifest.json";

        private CancellationTokenSource _cancellationTokenSource;

        public CancellationTokenSource CancellationTokenSource
        {
            get => _cancellationTokenSource;
            set
            {
                if (value != null && _cancellationTokenSource != null)
                {
                    throw new InvalidOperationException("The cancellation token is already set. Set it to null before using another.");
                }
                _cancellationTokenSource = value;
                TaskInProgress = _cancellationTokenSource != null;
            }
        }

        public bool TaskInProgress
        {
            get => this.btnCancel.Enabled;
            set
            {
                this.btnCancel.Enabled = value;
                this.pnlCrawl.Enabled = !value;
                this.pnlOptions.Enabled = !value;
                this.pnlSelectedPages.Enabled = !value;

                if (!value)
                {
                    this.progressBar.Value = 0;
                    this.progressBar.Maximum = 0;
                    this.lblStatus.Text = "";
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        // TODO: Make dynamic as options
        /// <summary>
        /// The device widths and descriptions/subdirectories for those device screenshots.
        /// </summary>
        private Dictionary<Device, int> _devices = null;

        private Dictionary<Device, int> Devices
        {
            get
            {
                _devices = _devices ?? new Dictionary<Device, int>()
                {
                    [Device.Desktop] = Properties.Settings.Default.DesktopWidth,
                    [Device.Mobile] = Properties.Settings.Default.MobileWidth,
                    [Device.Tablet] = Properties.Settings.Default.TabletWidth,
                };
                return _devices;
            }
            set
            {
                _devices = value;
                Utils.ChangeSettings(s =>
                {
                    s.DesktopWidth = _devices[Device.Desktop];
                    s.MobileWidth = _devices[Device.Mobile];
                    s.TabletWidth = _devices[Device.Tablet];
                });
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.txtBaseUrl.Text = Properties.Settings.Default.BaseUrl ?? "";
            this.txtOutputDir.Text = Properties.Settings.Default.OutputDir ?? "";
            RefreshDeviceControls();
        }

        private void SetDeviceWidth(Device device, int width)
        {
            Devices[device] = width;
            // Triggers a setting save in property setter..
            Devices = Devices;
        }

        private void RefreshDeviceControls()
        {
            this.numDesktopWidth.Value = Devices.ContainsKey(Device.Desktop) ? Devices[Device.Desktop] : 0;
            this.numMobileWidth.Value = Devices.ContainsKey(Device.Mobile) ? Devices[Device.Mobile] : 0;
            this.numTabletWidth.Value = Devices.ContainsKey(Device.Tablet) ? Devices[Device.Tablet] : 0;
        }

        private Settings GetSettings()
        {
            var settings = new Settings
            {
                RootUri = this.txtBaseUrl.Text,
                OutputDir = this.txtOutputDir.Text,
                CrawlExternalSites = this.cbCrawlExternalLinks.Checked,
                Devices = this.Devices,
                Uris = this.txtSelectedPages.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            };
            return settings;
        }

        private void LoadSettings(Settings settings)
        {
            this.txtBaseUrl.Text = settings.RootUri;
            this.txtOutputDir.Text = settings.OutputDir;
            this.cbCrawlExternalLinks.Checked = settings.CrawlExternalSites;
            Devices = settings.Devices;
            RefreshDeviceControls();
            this.txtSelectedPages.Text = string.Join(Environment.NewLine, settings.Uris);
        }

        private void BtnChooseOutput_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK != folderBrowserDialog1.ShowDialog())
            {
                return;
            }
            this.txtOutputDir.Text = folderBrowserDialog1.SelectedPath;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.txtOutputDir.Text))
            {
                this.saveSettingsDialog.InitialDirectory = this.txtOutputDir.Text;
            }
            if (DialogResult.OK != this.saveSettingsDialog.ShowDialog())
            {
                return;
            }

            var settings = GetSettings();
            var serializedSettings = JsonConvert.SerializeObject(settings);
            File.WriteAllText(
                this.saveSettingsDialog.FileName,
                serializedSettings);
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK != this.openFileDialog1.ShowDialog())
            {
                return;
            }

            if (!File.Exists(this.openFileDialog1.FileName))
            {
                MessageBox.Show("The settings file doesn't exist.");
                return;
            }

            var serializedSettings = File.ReadAllText(this.openFileDialog1.FileName);
            var settings = JsonConvert.DeserializeObject<Settings>(serializedSettings);
            LoadSettings(settings);
        }

        private void BtnStartCrawl_Click(object sender, EventArgs e)
        {
            var uri = this.txtBaseUrl.Text;
            if (uri.Length == 0)
            {
                MessageBox.Show("Please enter a root URL.");
                return;
            }

            this.pnlCrawl.Enabled = false;

            Utils.ChangeSettings(s => s.BaseUrl = uri);
            var spider = new Spider(uri, this.cbCrawlExternalLinks.Checked);

            CrawlResults pages = null;
            IProgress<ParsingProgress> progress = new Progress<ParsingProgress>(p =>
            {
                this.progressBar.Maximum = p.Count;
                this.progressBar.Value = p.CurrentIndex + 1;
                this.lblStatus.Text = p.CurrentItem;
            });
            CancellableAction(token =>
            {
                pages = spider.Crawl(token, progress);
            });
            SelectPages(pages);
            this.pnlCrawl.Enabled = true;
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

        private void SelectPages(CrawlResults pages)
        {
            var frm = new ChoosePagesForm(pages.VisitedUrls);
            if (DialogResult.OK != frm.ShowDialog(this))
            {
                return;
            }
            this.txtSelectedPages.Text = string.Join(Environment.NewLine, frm.IncludedPages);
        }

        private void BtnStartScreenshots_Click(object sender, EventArgs e)
        {
            var outputDir = this.txtOutputDir.Text;
            var manifestPath = Path.Combine(outputDir, ManifestFilename);

            if (string.IsNullOrWhiteSpace(outputDir))
            {
                outputDir = Path.GetDirectoryName(typeof(Form1).Assembly.Location);
            }

            if (File.Exists(manifestPath)
                && DialogResult.OK != MessageBox.Show(
                "This directory already has results. Do you want to overwrite them?",
                "Overwrite?",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2))
            {
                return;
            }

            Directory.CreateDirectory(outputDir);

            Utils.ChangeSettings(s => s.OutputDir = outputDir);
            TakeScreenshots(outputDir, manifestPath);
            LoadResults(manifestPath);
        }

        private void LoadResults(string manifestFile)
        {
            var frm = new ViewResultsForm(manifestFile);
            frm.ShowDialog();
        }

        private List<ScreenshotResult> TakeScreenshots(string outputDir, string manifestPath)
        {
            var results = new List<ScreenshotResult>();
            using (var ss = new Screenshotter())
            {
                try
                {
                    var uris = this.txtSelectedPages.Text
                        .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(u => new Uri(u));

                    foreach (var url in uris)
                    {
                        var result = new ScreenshotResult(url.ToString());
                        ScreenshotPageAsAllDevices(ss, url, result);
                        results.Add(result);
                    }
                }
                catch (UriFormatException ex)
                {
                    MessageBox.Show("Invalid Uri Provided: " + ex.Message);
                    return new List<ScreenshotResult>();
                }
            }

            var serializedResults = JsonConvert.SerializeObject(results);
            File.WriteAllText(manifestPath, serializedResults);
            return results;

            // LOCAL FUNCTIONS

            void ScreenshotPageAsAllDevices(Screenshotter ss, Uri url, ScreenshotResult result)
            {
                foreach (var size in _devices.Where(d => d.Value > 0))
                {
                    ScreenshotPageAsDevice(ss, url, result, size);
                }
            }

            void ScreenshotPageAsDevice(Screenshotter ss, Uri url, ScreenshotResult result, KeyValuePair<Device, int> size)
            {
                var device = size.Key;
                var width = size.Value;
                var filenameUriComponent = Screenshotter.SanitizeFilename(url.ToString());
                var filename = $"{filenameUriComponent}.{device}{Screenshotter.ImageExtension}";
                var relPath = Path.Combine("images", filename);
                var path = Path.Combine(outputDir, relPath);

                try
                {
                    ss.TakeScreenshot(url.ToString(), path, width);
                    result.Paths.Add(device, relPath);
                }
                catch (Exception ex)
                {
                    result.Error += ex.Message + Environment.NewLine;
                }
            }
        }

        private void BtnViewResults_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.txtOutputDir.Text))
            {
                this.openManifestDialog.InitialDirectory = this.txtOutputDir.Text;
            }

            if (DialogResult.OK == this.openManifestDialog.ShowDialog()
                && File.Exists(this.openManifestDialog.FileName))
            {
                LoadResults(this.openManifestDialog.FileName);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource = null;
        }

        private void DeviceWidth_Changed(object sender, EventArgs e)
        {
            NumericUpDown widthControl = (NumericUpDown)sender;
            var deviceName = (string)widthControl.Tag;
            var device = (Device)Enum.Parse(typeof(Device), deviceName);
            SetDeviceWidth(device, (int)widthControl.Value);
        }
    }
}