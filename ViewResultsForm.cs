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

                if (_selectedImage != null && !File.Exists(path))
                {
                    MessageBox.Show("The selected image file doesn't exist.");
                    _selectedImage = null;
                }

                if (_selectedImage != null)
                {
                    this.lnkUrl.Text = _selectedImage.Result.Uri;
                    this.pictureBox1.Image = Image.FromFile(path);
                    this.btnShowInExplorer.Enabled = true;
                    ApplyImageZoom();
                }
                else
                {
                    this.lnkUrl.Text = "";
                    this.pictureBox1.Image?.Dispose();
                    this.pictureBox1.Image = null;
                    this.btnShowInExplorer.Enabled = false;
                }
            }
        }

        private List<ScreenshotResult> _results;
        private readonly string _mostCommonHost;

        private Point MouseDownLocation;

        public ViewResultsForm()
        {
            InitializeComponent();
            SelectedImage = null;
        }

        public ViewResultsForm(string manifestPath) : this()
        {
            ManifestPath = manifestPath;

            if (!File.Exists(ManifestPath))
            {
                throw new FileNotFoundException(ManifestPath);
            }

            var serializedResults = File.ReadAllText(ManifestPath);
            _results = JsonConvert.DeserializeObject<List<ScreenshotResult>>(serializedResults);

            // Hide the most common host to avoid visual clutter
            _mostCommonHost = _results.GroupBy(x => new Uri(x.Uri).Host)
                .Select(x => new { x.Key, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .Select(x => x.Key)
                .FirstOrDefault() ?? "";
        }

        private string RemoveCommonHost(string link)
        {
            var uri = new Uri(link);
            var host = uri.Host;
            return string.Equals(host, _mostCommonHost, StringComparison.OrdinalIgnoreCase)
                ? uri.PathAndQuery : link;
            //Regex.Replace(link, $@"https?://{_mostCommonHost}", "", RegexOptions.IgnoreCase);
        }

        private void ViewResultsForm_Load(object sender, EventArgs e)
        {
            AddByUriNode();
            AddByDeviceNode();
            this.treeFiles.ExpandAll();
            this.trackZoom.Value = 100;

            this.pnlPicture.MouseWheel += PnlPicture_MouseWheel;
        }

        private void PnlPicture_MouseWheel(object sender, MouseEventArgs e)
        {
            MoveImage(dY: e.Delta);
        }

        private void ResizeScrollbars()
        {
            this.vscrollImg.Maximum = Math.Max(0, this.pictureBox1.Height - this.pnlPicture.ClientSize.Height + this.vscrollImg.LargeChange);
            this.hscrollImg.Maximum = Math.Max(0, this.pictureBox1.Width - this.pnlPicture.ClientSize.Width + this.hscrollImg.LargeChange);
        }

        private void AddByUriNode()
        {
            var rootNode = new TreeNode("By Web Page");
            _results.ForEach(r =>
            {
                var uri = new Uri(r.Uri);

                var childrenNodes = r.Paths
                    .Select(p => new TreeNode(p.Key.ToString())
                    {
                        Tag = new ScreenshotFile
                        {
                            Device = p.Key,
                            Result = r
                        }
                    })
                    .Cast<TreeNode>()
                    .ToArray();

                var node = new TreeNode(RemoveCommonHost(r.Uri), childrenNodes)
                {
                    ToolTipText = r.Uri,
                    NodeFont = new Font(this.treeFiles.Font, FontStyle.Bold)
                };

                rootNode.Nodes.Add(node);
            });

            this.treeFiles.Nodes.Add(rootNode);
        }

        private void AddByDeviceNode()
        {
            var rootNode = new TreeNode("Device Widths");
            Enum.GetValues(typeof(Device))
                .Cast<Device>()
                .ForEach(device =>
                {
                    var deviceNode = new TreeNode(device.ToString());
                    rootNode.Nodes.Add(deviceNode);

                    var pageNodes = _results
                        .Where(r => r.Paths.ContainsKey(device))
                        .Select(r => new TreeNode(RemoveCommonHost(r.Uri))
                        {
                            Tag = new ScreenshotFile
                            {
                                Device = device,
                                Result = r
                            },
                            ToolTipText = r.Uri,
                            NodeFont = new Font(this.treeFiles.Font, FontStyle.Bold)
                        }).ToArray();

                    deviceNode.Nodes.AddRange(pageNodes);
                });
            this.treeFiles.Nodes.Add(rootNode);
        }

        private void TreeFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedImage = e.Node.Tag as ScreenshotFile;
        }

        private void LnkUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var uri = this.lnkUrl.Text;
            if (uri.Length == 0)
            {
                return;
            }
            Process.Start(uri);
        }

        private void BtnShowInExplorer_Click(object sender, EventArgs e)
        {
            ShowSelectedImageInExplorer();
        }

        private void ShowSelectedImageInExplorer()
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
                MouseDownLocation = e.Location;
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.pictureBox1.Image != null && e.Button == MouseButtons.Left)
            {
                MoveImage(e.X - MouseDownLocation.X, e.Y - MouseDownLocation.Y);
            }
        }

        private int InRange(int value, int min, int max) =>
            Math.Min(max, Math.Max(min, value));

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void ApplyImageZoom()
        {
            var value = this.trackZoom.Value;
            this.lblZoom.Text = $"Zoom ({value}%):";
            if (this.pictureBox1.Image == null)
            {
                return;
            }
            double factor = value / 100.0;
            var imgSize = this.pictureBox1.Image.Size;

            var scaledSize = new Size(
                (int)Math.Round(factor * imgSize.Width),
                (int)Math.Round(factor * imgSize.Height));

            if (this.cbImageAutoWidth.Checked && scaledSize.Width > this.pnlPicture.ClientSize.Width)
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
            ApplyImageZoom();
        }

        private bool AutoScrollImage
        {
            get => this.timerScroll.Enabled;
            set
            {
                this.btnToggleAutoScroll.Text = value ? "Stop Autoscroll" : "Start Autoscroll";
                this.btnToggleAutoScroll.BackColor = value ? Color.LightGreen : Color.Pink;
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
            if (this.cbImageAutoWidth.Checked)
            {
                ApplyImageZoom();
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
                Console.WriteLine($"pictureBox1.Top={this.pictureBox1.Top}, ScrollVal={this.vscrollImg.Value}, ScrollMax={this.vscrollImg.Maximum}, panelHeight={this.pnlPicture.ClientSize.Height}");

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
            ApplyImageZoom();
        }
    }
}