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
                if (_selectedImage != null)
                {
                    var path = _selectedImage.GetPath(OutputDirectory);
                    if (File.Exists(path))
                    {
                        this.lnkUrl.Text = _selectedImage.Result.Uri;
                        this.pictureBox1.Image = Image.FromFile(path);
                        this.btnShowInExplorer.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("The selected image file doesn't exist.");
                    }
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
        }

        private void ViewResults_Load(object sender, EventArgs e)
        {
            AddByUriNode();
            AddByDeviceNode();
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

                var node = new TreeNode(r.Uri, childrenNodes);

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
                        .Select(r => new TreeNode(r.Uri)
                        {
                            Tag = new ScreenshotFile
                            {
                                Device = device,
                                Result = r
                            }
                        }).ToArray();

                    deviceNode.Nodes.AddRange(pageNodes);
                });
            this.treeFiles.Nodes.Add(rootNode);
        }

        private void treeFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedImage = e.Node.Tag as ScreenshotFile;
        }

        private void LnkUrl_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void lnkUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var uri = this.lnkUrl.Text;
            if (uri.Length == 0)
            {
                return;
            }
            System.Diagnostics.Process.Start(uri);
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
    }
}