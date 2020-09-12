using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Webshot.Screenshots;

namespace Webshot.Controls
{
    public partial class ViewResultsControl : UserControl
    {
        private ScreenshotFile _selectedImage;

        private Dictionary<Device, int> _deviceScales;

        public ViewerOptions Options
        {
            get => new ViewerOptions
            {
                DeviceOutputScales = _deviceScales ?? new ViewerOptions().DeviceOutputScales,
                ConstrainImageWidth = ConstrainImageWidth,
            };
            set
            {
                _deviceScales = value.DeviceOutputScales;
                ImageScale = _deviceScales != null && SelectedImage != null
                    ? _deviceScales[SelectedImage.Device] / 100.0
                    : 1.0;
                ConstrainImageWidth = value.ConstrainImageWidth;
            }
        }

        /// <summary>
        /// The primary screenshot visible to the user.
        /// </summary>
        private ScreenshotFile SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;

                var path = _selectedImage?.GetPath("OUTPUT DIR");

                if (_selectedImage != null)
                {
                    if (!File.Exists(path))
                    {
                        MessageBox.Show("The selected image file doesn't exist.");
                        ClearImage();
                        return;
                    }
                    SetImage();
                }
                else
                {
                    ClearImage();
                }

                void SetImage()
                {
                    this.imageViewer.Image = Image.FromFile(path);
                }

                void ClearImage()
                {
                    this.imageViewer.Image = null;
                }
            }
        }

        public double ImageScale
        {
            get => this.trackZoom.Value / 100.0;
            set
            {
                int scale = (int)Math.Round(value * 100);
                this.trackZoom.Value = scale;
                this.imageViewer.ImageScale = scale;
            }
        }

        public bool ConstrainImageWidth
        {
            get => this.cbFitImage.Checked;
            set
            {
                this.cbFitImage.Checked = value;
                this.imageViewer.ConstrainImageWidth = value;
            }
        }

        public void RefreshScreenshotList()
        {
            SelectedImage = null;
            this.screenshotTree1.SetResultSet(_results);
        }

        private ScreenshotResults _results = new ScreenshotResults();

        public ViewResultsControl()
        {
            InitializeComponent();
            RefreshScreenshotList();
            InitControls();
        }

        private void ViewResultsControl_Load(object sender, EventArgs e)
        {
        }

        private void InitControls()
        {
            RefreshScreenshotList();
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
        }

        private bool AutoScrollImage
        {
            get => false;
            set
            {
                this.btnToggleAutoScroll.Text =
                    value ? "Stop Autoscroll" : "Start Autoscroll";
                this.btnToggleAutoScroll.BackColor =
                    value ? Color.Pink : Color.LightGreen;
                //this.timerScroll.Enabled = value;
            }
        }

        private void BtnToggleAutoScroll_Click(object sender, EventArgs e)
        {
            AutoScrollImage = !AutoScrollImage;
        }

        private void ComboZoomDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ComboResultSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshScreenshotList();
        }

        private void CbFitImage_CheckedChanged(object sender, EventArgs e)
        {
            this.imageViewer.ConstrainImageWidth = this.cbFitImage.Checked;
        }

        private void trackScrollSpeed_Scroll(object sender, EventArgs e)
        {
        }

        private void ScreenshotTree1_ScreenshotSelected(object sender, EventArgs e)
        {
            SelectedImage = this.screenshotTree1.SelectedFile;
        }
    }
}