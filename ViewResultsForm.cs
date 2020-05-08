using Newtonsoft.Json;
using OpenQA.Selenium.DevTools.DOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

                this.pictureBox1.Top = 0;
                this.pictureBox1.Left = 0;

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
                    ApplyImageZoom(this.trackZoom.Value);
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

        private void ViewResults_Load(object sender, EventArgs e)
        {
            AddByUriNode();
            AddByDeviceNode();
            this.treeFiles.ExpandAll();
            this.trackZoom.Value = 100;
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

        private void treeFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedImage = e.Node.Tag as ScreenshotFile;
        }

        private void lnkUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var uri = this.lnkUrl.Text;
            if (uri.Length == 0)
            {
                return;
            }
            Process.Start(uri);
        }

        private void btnShowInExplorer_Click(object sender, EventArgs e)
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

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Cursor = Cursors.NoMove2D;
                MouseDownLocation = e.Location;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.pictureBox1.Image != null && e.Button == MouseButtons.Left)
            {
                // Where the image would be w/o any constraints
                var rawLocation = new Point(
                    e.X + this.pictureBox1.Left - MouseDownLocation.X,
                    e.Y + this.pictureBox1.Top - MouseDownLocation.Y);

                // Constrain image movement so it's always visible.
                var imgSize = this.pictureBox1.ClientSize;
                var containerSize = this.pnlPicture.ClientSize;
                var xRange = Math.Max(0, imgSize.Width - containerSize.Width);
                var yRange = Math.Max(0, imgSize.Height - containerSize.Height);

                this.pictureBox1.Left = InRange(rawLocation.X, -xRange, 0);
                this.pictureBox1.Top = InRange(rawLocation.Y, -yRange, 0);
            }
        }

        private int InRange(int value, int min, int max) =>
            Math.Min(max, Math.Max(min, value));

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void ApplyImageZoom(int value)
        {
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
            this.pictureBox1.Location = new Point(0, 0);
            this.pictureBox1.Size = scaledSize;
        }

        private void trackZoom_Scroll(object sender, EventArgs e)
        {
            ApplyImageZoom(this.trackZoom.Value);
        }
    }
}