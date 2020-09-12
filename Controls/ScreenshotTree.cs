using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Webshot.Screenshots;

namespace Webshot.Controls
{
    public partial class ScreenshotTree : UserControl
    {
        public event EventHandler ScreenshotSelected;

        public ScreenshotTree()
        {
            InitializeComponent();
        }

        public SiteLegend SiteLegend { get; private set; }

        private ScreenshotResults _results;

        private List<DeviceScreenshots> Screenshots =>
            _results?.Screenshots ?? new List<DeviceScreenshots>(0);

        public void SetResultSet(ScreenshotResults resultSet)
        {
            _results = resultSet;
            UpdateLegend();
            this.treeFiles.Nodes.Clear();
            AddNode(ByDeviceNode(), true);
            AddNode(ByUriNode());
        }

        private void AddNode(TreeNode node, bool expand = false)
        {
            this.treeFiles.Nodes.Add(node);
            if (expand)
            {
                node.ExpandAll();
            }
        }

        private void UpdateLegend()
        {
            var uris = Screenshots.Select(s => s.Uri);
            SiteLegend = new SiteLegend(uris);
            this.lblLegend.Text = SiteLegend.ToString();
        }

        private TreeNode ByUriNode()
        {
            var categoryNode = new TreeNode("By Web Page");
            Screenshots.ForEach(r =>
            {
                var childrenNodes = r.Paths
                    .Select(p => new TreeNode(p.Key.ToString())
                    {
                        Tag = new ScreenshotFile(r, p.Key)
                    })
                    .ToArray();

                string shortUri = SiteLegend.CondenseUri(r.Uri);
                var pageNode = new TreeNode(shortUri, childrenNodes)
                {
                    ToolTipText = r.Uri.ToString(),
                    NodeFont = new Font(Font, FontStyle.Bold)
                };

                categoryNode.Nodes.Add(pageNode);
            });

            return categoryNode;
        }

        private TreeNode ByDeviceNode()
        {
            var categoryNode = new TreeNode("Device Widths");
            Enum.GetValues(typeof(Device))
                .Cast<Device>()
                .ForEach(device =>
                {
                    var deviceNode = new TreeNode(device.ToString());
                    categoryNode.Nodes.Add(deviceNode);
                    var pageNodes = Screenshots
                        .Where(r => r.Paths.ContainsKey(device))
                        .Select(r => new TreeNode(SiteLegend.CondenseUri(r.Uri))
                        {
                            Tag = new ScreenshotFile(r, device),
                            ToolTipText = r.Uri.ToString(),
                            NodeFont = new Font(Font, FontStyle.Bold)
                        })
                        .ToArray();

                    deviceNode.Nodes.AddRange(pageNodes);
                    if (device == Device.Desktop)
                    {
                        deviceNode.Expand();
                    }
                });
            return categoryNode;
        }

        public ScreenshotFile SelectedFile
        {
            get => this.treeFiles.SelectedNode?.Tag as ScreenshotFile;
            set
            {
                this.treeFiles.SelectedNode =
                    this.treeFiles.Nodes
                        .Cast<TreeNode>()
                        .FirstOrDefault(n => n.Tag == value);
            }
        }

        private void TreeFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            OnScreenshotSelected();
        }

        protected void OnScreenshotSelected()
        {
            ScreenshotSelected?.Invoke(this, EventArgs.Empty);
        }
    }
}