using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Webshot
{
    public partial class ViewResultsForm : Form
    {
        private class ScreenshotFile
        {
            public ScreenshotFile(ScreenshotResult result, Device device)
            {
                this.Result = result ?? throw new ArgumentNullException(nameof(result));
                this.Device = device;
            }

            public ScreenshotResult Result { get; set; }
            public Device Device { get; set; }

            public String GetPath(string basePath) => Path.Combine(basePath, Result.Paths[Device]);
        }

        public string ManifestPath { get; set; }
        public string OutputDirectory => Path.GetDirectoryName(ManifestPath);

        private ScreenshotFile _selectedImage;

        private ScreenshotFile SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;

                var path = _selectedImage?.GetPath(OutputDirectory);

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
                    this.lnkUrl.Text = _selectedImage.Result.Uri;
                    this.pictureBox1.Image = Image.FromFile(path);
                    this.btnShowInExplorer.Enabled = true;
                    ZoomImage();
                }

                void ClearImage()
                {
                    this.lnkUrl.Text = "";
                    this.pictureBox1.Image?.Dispose();
                    this.pictureBox1.Image = null;
                    this.btnShowInExplorer.Enabled = false;
                }
            }
        }

        private readonly List<ScreenshotResult> _results;

        /// <summary>
        /// Key=host name, Value=footnote identifer
        /// </summary>
        private readonly Dictionary<string, string> _hosts;

        private Point _mouseDownLocation;

        public ViewResultsForm()
        {
            InitializeComponent();
            SelectedImage = null;
        }

        public ViewResultsForm(string manifestPath) : this()
        {
            if (!File.Exists(manifestPath))
            {
                throw new FileNotFoundException(ManifestPath);
            }

            ManifestPath = manifestPath;

            var serializedManifest = File.ReadAllText(ManifestPath);
            _results = JsonConvert.DeserializeObject<List<ScreenshotResult>>(serializedManifest);

            // Hide the most common host in display elements to avoid visual clutter.
            _hosts = _results
                .Select(r => GetHostKey(new Uri(r.Uri)))
                .Distinct()
                .Select((h, i) => new { Host = h, Index = i })
                .ToDictionary(x => x.Host, x => x.Index.ToString());

            this.lblLegend.Text = string.Join(
                Environment.NewLine,
                _hosts.Select(h => $"[{h.Value}] = {h.Key}").ToArray());
        }

        private string GetHostKey(Uri uri) => uri.Host.ToUpperInvariant();

        public string ShortenedUri(string uri) => ShortenedUri(new Uri(uri));

        public string ShortenedUri(Uri uri) =>
            $"[{_hosts[GetHostKey(uri)]}]{uri.PathAndQuery}";

        private void ViewResultsForm_Load(object sender, EventArgs e)
        {
            AddByUriNode();
            AddByDeviceNode();
            // Force label update
            this.trackZoom.Value = 100;
            this.pnlPicture.MouseWheel += PnlPicture_MouseWheel;
        }

        private void PnlPicture_MouseWheel(object sender, MouseEventArgs e)
        {
            MoveImage(dY: e.Delta);
        }

        private void ResizeScrollbars()
        {
            var extraPic = Size.Subtract(
                this.pictureBox1.Size,
                this.pnlPicture.ClientSize);

            var scrollMargin = new Size(
                this.hscrollImg.LargeChange,
                this.vscrollImg.LargeChange);

            var maxScroll = Size.Add(extraPic, scrollMargin);

            this.vscrollImg.Maximum = Math.Max(0, maxScroll.Height);
            this.hscrollImg.Maximum = Math.Max(0, maxScroll.Width);
        }

        private void AddByUriNode()
        {
            var categoryNode = new TreeNode("By Web Page");
            _results.ForEach(r =>
            {
                var childrenNodes = r.Paths
                    .Select(p => new TreeNode(p.Key.ToString())
                    {
                        Tag = new ScreenshotFile(r, p.Key)
                    })
                    .ToArray();

                var pageNode = new TreeNode(ShortenedUri(r.Uri), childrenNodes)
                {
                    ToolTipText = r.Uri,
                    NodeFont = new Font(this.treeFiles.Font, FontStyle.Bold)
                };

                categoryNode.Nodes.Add(pageNode);
            });

            this.treeFiles.Nodes.Add(categoryNode);
        }

        private void AddByDeviceNode()
        {
            var categoryNode = new TreeNode("Device Widths");
            Enum.GetValues(typeof(Device))
                .Cast<Device>()
                .ForEach(device =>
                {
                    var deviceNode = new TreeNode(device.ToString());
                    categoryNode.Nodes.Add(deviceNode);

                    var pageNodes = _results
                        .Where(r => r.Paths.ContainsKey(device))
                        .Select(r => new TreeNode(ShortenedUri(r.Uri))
                        {
                            Tag = new ScreenshotFile(r, device),
                            ToolTipText = r.Uri,
                            NodeFont = new Font(this.treeFiles.Font, FontStyle.Bold)
                        })
                        .ToArray();

                    deviceNode.Nodes.AddRange(pageNodes);
                    if (device == Device.Desktop)
                    {
                        deviceNode.Expand();
                    }
                });
            this.treeFiles.Nodes.Add(categoryNode);
            categoryNode.Expand();
        }

        private void TreeFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedImage = e.Node.Tag as ScreenshotFile;
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
            string path = SelectedImage.GetPath(OutputDirectory);

            // From: https://stackoverflow.com/a/9904834
            string args = string.Format("/e, /select, \"{0}\"", path);

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = "explorer",
                Arguments = args
            };
            Process.Start(info);
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Cursor = Cursors.NoMove2D;
                _mouseDownLocation = e.Location;
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.pictureBox1.Image != null && e.Button == MouseButtons.Left)
            {
                MoveImage(e.X - _mouseDownLocation.X, e.Y - _mouseDownLocation.Y);
            }
        }

        private int InRange(int value, int min, int max) =>
            Math.Min(max, Math.Max(min, value));

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void ZoomImage()
        {
            var selectedZoom = this.trackZoom.Value;
            this.lblZoom.Text = $"Zoom ({selectedZoom}%):";

            if (this.pictureBox1.Image == null)
            {
                return;
            }
            double zoomFactor = selectedZoom / 100.0;
            var imgSize = this.pictureBox1.Image.Size;

            var scaledSize = new Size(
                (int)Math.Round(zoomFactor * imgSize.Width),
                (int)Math.Round(zoomFactor * imgSize.Height));

            if (this.cbFitImage.Checked && scaledSize.Width > this.pnlPicture.ClientSize.Width)
            {
                var containerScale = (double)this.pnlPicture.ClientSize.Width / scaledSize.Width;
                scaledSize = new Size(
                    (int)Math.Floor(containerScale * scaledSize.Width),
                    (int)Math.Floor(containerScale * scaledSize.Height));
            }

            this.pictureBox1.Size = scaledSize;
            ResizeScrollbars();
            ImageLocation = new Point(0, 0);
        }

        private void TrackZoom_Scroll(object sender, EventArgs e)
        {
            ZoomImage();
        }

        private bool AutoScrollImage
        {
            get => this.timerScroll.Enabled;
            set
            {
                this.btnToggleAutoScroll.Text = value ? "Stop Autoscroll" : "Start Autoscroll";
                this.btnToggleAutoScroll.BackColor = value ? Color.Pink : Color.LightGreen;
                this.timerScroll.Enabled = value;
            }
        }

        private void BtnToggleAutoScroll_Click(object sender, EventArgs e)
        {
            AutoScrollImage = !AutoScrollImage;
        }

        private void TimerScroll_Tick(object sender, EventArgs e)
        {
            PerformAutoScroll();
        }

        private void PerformAutoScroll()
        {
            if (this.pictureBox1.Image != null && this.vscrollImg.Value < this.vscrollImg.Maximum)
            {
                MoveImage(dY: -this.trackScrollSpeed.Value);
            }
        }

        private void VscrollImg_Scroll(object sender, ScrollEventArgs e)
        {
            ImageLocation = new Point(ImageLocation.X, -this.vscrollImg.Value);
        }

        private void HscrollImg_Scroll(object sender, ScrollEventArgs e)
        {
            ImageLocation = new Point(-this.hscrollImg.Value, ImageLocation.Y);
        }

        private void PnlPicture_Resize(object sender, EventArgs e)
        {
            if (this.cbFitImage.Checked)
            {
                ZoomImage();
            }
            ResizeScrollbars();
        }

        private void MoveImage(int dX = 0, int dY = 0)
        {
            var translation = new Size(dX, dY);
            ImageLocation = Point.Add(ImageLocation, translation);
        }

        private Point ImageLocation
        {
            get => this.pictureBox1.Location;
            set
            {
                var constrainedLocation = new Point(
                    InRange(value.X, this.pnlPicture.ClientSize.Width - this.pictureBox1.Width, 0),
                    InRange(value.Y, this.pnlPicture.ClientSize.Height - this.pictureBox1.Height, 0));

                if (this.pictureBox1.Location == constrainedLocation)
                {
                    return;
                }

                this.pictureBox1.Location = constrainedLocation;
                this.vscrollImg.Value = -constrainedLocation.Y;
                this.hscrollImg.Value = -constrainedLocation.X;
            }
        }

        private void CbImageAutoWidth_CheckedChanged(object sender, EventArgs e)
        {
            ZoomImage();
        }
    }
}