using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Webshot.Controls;
using Webshot.Screenshots;

namespace Webshot
{
    public partial class ViewResultsForm : Form
    {
        public event EventHandler OptionsChanged;

        public event EventHandler ScreenshotSelected;

        private ScreenshotFile _selectedImage;

        /// <summary>
        /// The primary screenshot visible to the user.
        /// </summary>
        public ScreenshotFile SelectedImage
        {
            get => _selectedImage;

            private set
            {
                if (_selectedImage == value)
                {
                    return;
                }
                _selectedImage = value;
                this.lnkUrl.Text = value?.Result?.Uri?.ToString() ?? "";
                RefreshZoomForCurrentDevice();
                OnScreenshotSelected();
            }
        }


        public Image DisplayedImage
        {
            get => this.imageViewer.Image;
            set { this.imageViewer.Image = value; }
        }

        private ViewerOptions _options = new ViewerOptions();

        public ViewerOptions Options
        {
            get => _options;
            set
            {
                _options = value;
                ImageZoom = _options.GetScale(CurrentDevice);
                ConstrainImageWidth = _options.ConstrainImageWidth;
            }
        }

        private Device CurrentDevice => SelectedImage?.Device ?? Device.Desktop;

        public int ImageZoom
        {
            get => this.imageViewer.ImageScale;
            set
            {
                const int min = LargeImageViewer.MinScale;
                const int max = LargeImageViewer.MaxScale;

                if (value < min || value > max)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(ImageZoom),
                        value,
                        $"Value must be between {min} and {max}.");
                }
                this.trackZoom.Value = value;
                this.imageViewer.ImageScale = value;
                _options.DeviceOutputScales[CurrentDevice] = value;
                RefreshZoomLabel();
                OnOptionsChanged();
            }
        }

        public bool ConstrainImageWidth
        {
            get => this.cbFitImage.Checked;
            set
            {
                this.cbFitImage.Checked = value;
                this.imageViewer.ConstrainImageWidth = value;
                _options.ConstrainImageWidth = value;
                OnOptionsChanged();
            }
        }

        protected void OnOptionsChanged()
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        protected void OnScreenshotSelected()
        {
            ScreenshotSelected?.Invoke(this, EventArgs.Empty);
        }

        private ScreenshotResults _results = new ScreenshotResults();

        public ScreenshotResults Results
        {
            get => _results;
            set
            {
                _results = value;
                this.treeScreenshots.SetResultSet(value);
            }
        }

        public ViewResultsForm()
        {
            InitializeComponent();
            SelectedImage = null;
            this.trackZoom.Minimum = LargeImageViewer.MinScale;
            this.trackZoom.Maximum = LargeImageViewer.MaxScale;
        }

        /// <summary>
        /// Resets the zoom control for the current screenshot device type.
        /// </summary>
        private void RefreshZoomForCurrentDevice()
        {
            int settingZoom = _options.DeviceOutputScales[CurrentDevice];
            ImageZoom = settingZoom;
        }

        private void RefreshZoomLabel()
        {
            this.lblZoom.Text = $"{CurrentDevice} Zoom ({ImageZoom}):";
        }

        private void ViewResultsForm_Load(object sender, EventArgs e)
        {
            // Force label update
            this.trackZoom.Value = 100;
        }

        private void LnkUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var uri = this.lnkUrl.Text;
            if (uri.Length > 0)
            {
                Process.Start(uri);
            }
        }

        private void BtnShowInExplorer_Click(object sender, EventArgs e)
        {
            string path = SelectedImage.GetPath("OutputDirectory");

            // From: https://stackoverflow.com/a/9904834
            string args = string.Format("/e, /select, \"{0}\"", path);

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = "explorer",
                Arguments = args
            };
            Process.Start(info);
        }

        private void TrackZoom_Scroll(object sender, EventArgs e)
        {
            ImageZoom = this.trackZoom.Value;
        }

        private bool AutoScrollImage
        {
            get => this.imageViewer.AutoscrollImage;
            set
            {
                this.imageViewer.AutoscrollImage = value;
                this.imageViewer.AutoScrollImageSpeed = this.trackScrollSpeed.Value;
                this.btnToggleAutoScroll.Text =
                    value ? "Stop Autoscroll" : "Start Autoscroll";
                this.btnToggleAutoScroll.BackColor =
                    value ? Color.Pink : Color.LightGreen;
            }
        }

        private void BtnToggleAutoScroll_Click(object sender, EventArgs e)
        {
            AutoScrollImage = !AutoScrollImage;
        }

        private void CbFitImage_CheckedChanged(object sender, EventArgs e)
        {
            this.imageViewer.ConstrainImageWidth = this.cbFitImage.Checked;
        }

        private void ScreenshotTree1_ScreenshotSelected(object sender, EventArgs e)
        {
            this.SelectedImage = this.treeScreenshots.SelectedFile;
        }

        private void TrackScrollSpeed_Scroll(object sender, EventArgs e)
        {
            this.imageViewer.AutoScrollImageSpeed = this.trackScrollSpeed.Value;
        }
    }
}