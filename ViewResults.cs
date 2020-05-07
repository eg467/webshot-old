using Newtonsoft.Json;
using OpenQA.Selenium.DevTools.DOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public string ManifestPath { get; set; }
        public string OutputDirectory => Path.GetDirectoryName(ManifestPath);

        private List<ScreenshotResult> _results;

        public ViewResultsForm()
        {
            InitializeComponent();
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
            _results.ForEach(r =>
            {
                var uri = new Uri(r.Uri);

                var childrenNodes = r.Paths
                    .Select(p => new TreeNode(p.Key.ToString())
                    {
                        Tag = p.Value
                    })
                    .Cast<TreeNode>()
                    .ToArray();

                var node = new TreeNode(r.Uri, childrenNodes)
                {
                    Tag = uri
                };

                treeFiles.Nodes.Add(node);
            });
        }

        private void treeFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Uri)
            {
                return;
            }

            var relPath = (string)e.Node.Tag;
            var absPath = Path.Combine(OutputDirectory, relPath);

            if (File.Exists(absPath))
            {
                this.pictureBox1.Image = Image.FromFile(absPath);
            }
        }
    }
}