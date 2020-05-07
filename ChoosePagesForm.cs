using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Webshot
{
    public partial class ChoosePagesForm : Form
    {
        private readonly Dictionary<Uri, bool> _pages;

        public IEnumerable<Uri> IncludedPages =>
            _pages.Where(p => p.Value)
                .Select(k => k.Key)
                .OrderBy(k => k.ToString());

        public ChoosePagesForm(IEnumerable<Uri> uris) : this()
        {
            _pages = uris.ToDictionary(k => k, v => false);
        }

        public ChoosePagesForm()
        {
            InitializeComponent();
        }

        private void ChoosePagesForm_Load(object sender, EventArgs e)
        {
            var uris = _pages.Keys.OrderBy(k => k.ToString()).ToArray();
            this.cblAvailableUris.Items.AddRange(uris);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            SetAll(true);
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            SetAll(false);
        }

        private void SetAll(bool value)
        {
            Enumerable.Range(0, this.cblAvailableUris.Items.Count)
                .ForEach(i => this.cblAvailableUris.SetItemChecked(i, value));
        }

        private void cblAvailableUris_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cblAvailableUris_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var uri = (Uri)this.cblAvailableUris.Items[e.Index];
            this._pages[uri] = e.NewValue == CheckState.Checked;
        }
    }
}