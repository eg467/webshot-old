using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace Webshot
{
    public partial class ChoosePagesForm : Form
    {
        private readonly Dictionary<Uri, bool> _pages;
        private readonly IReadOnlyDictionary<Uri, BrokenLink> _brokenLinks;

        public IEnumerable<Uri> IncludedPages =>
            _pages.Where(p => p.Value)
                .Select(k => k.Key)
                .OrderBy(k => k.ToString());

        public ChoosePagesForm(CrawlResults results) : this()
        {
            _pages = results.SitePages.ToDictionary(k => k, v => false);
            _brokenLinks = results.BrokenLinks;
        }

        public ChoosePagesForm()
        {
            InitializeComponent();
        }

        private void ChoosePagesForm_Load(object sender, EventArgs e)
        {
            DisplayAvailablePages();
            DisplayErrors();
        }

        private void DisplayAvailablePages()
        {
            var uris = _pages.Keys.OrderBy(k => k.ToString()).ToArray();
            this.cblAvailableUris.Items.AddRange(uris);
            SetAll(true);
        }

        private void DisplayErrors()
        {
            _brokenLinks
           .Select(x => new
           {
               Uri = x.Key,
               x.Value.ExceptionMessage
           })
           .ForEach(x =>
           {
               var link = new LinkLabel()
               {
                   Text = x.Uri.ToString(),
               };
               link.Click += UriLinkClicked;

               var lblMessage = new Label()
               {
                   Text = x.ExceptionMessage
               };

               var lvi = new ListViewItem(new string[] { x.Uri.ToString(), x.ExceptionMessage });
               this.lvErrors.Items.Add(lvi);
           });
        }

        private void UriLinkClicked(object sender, EventArgs e)
        {
            var uri = ((LinkLabel)sender).Text;
            System.Diagnostics.Process.Start(uri);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            SetAll(true);
        }

        private void BtnSelectNone_Click(object sender, EventArgs e)
        {
            SetAll(false);
        }

        private void SetAll(bool value)
        {
            Enumerable.Range(0, this.cblAvailableUris.Items.Count)
                .ForEach(i => this.cblAvailableUris.SetItemChecked(i, value));
        }

        private void CblAvailableUris_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void CblAvailableUris_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var uri = (Uri)this.cblAvailableUris.Items[e.Index];
            this._pages[uri] = e.NewValue == CheckState.Checked;
        }

        private void LvErrors_DoubleClick(object sender, EventArgs e)
        {
            if (this.lvErrors.SelectedItems.Count > 0)
            {
                var uri = this.lvErrors.SelectedItems[0].SubItems[0].Text;
                System.Diagnostics.Process.Start(uri);
            }
        }
    }
}