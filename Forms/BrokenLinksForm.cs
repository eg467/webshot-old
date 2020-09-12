using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Webshot.Forms
{
    public partial class BrokenLinksForm : Form
    {
        public BrokenLinksForm()
        {
            InitializeComponent();
        }

        public void UpdateLinks(List<BrokenLink> links)
        {
            this.treeLinks.Nodes.Clear();

            TreeNode CreateNode(BrokenLink link) =>
                new TreeNode(link.Target.AbsoluteUri, GetChildNodes(link))
                {
                    Tag = link.Target.AbsoluteUri,
                };

            TreeNode[] GetChildNodes(BrokenLink link) =>
              link.Sources
                  .OrderBy(x => x.CallingPage.AbsoluteUri)
                  .Select(s =>
                      new TreeNode($"{s.CallingPage} [{s.Href}]")
                      {
                          Tag = s.CallingPage.AbsoluteUri,
                      })
                  .ToArray();

            TreeNode[] linkNodes = links.Select(CreateNode).ToArray();
            this.treeLinks.Nodes.AddRange(linkNodes);
        }

        private void treeLinks_DoubleClick(object sender, EventArgs e)
        {
            var uri = this.treeLinks.SelectedNode?.Tag as string;
            if (string.IsNullOrEmpty(uri)) return;
            System.Diagnostics.Process.Start(uri);
        }

        private void BrokenLinksForm_Load(object sender, EventArgs e)
        {
        }
    }
}