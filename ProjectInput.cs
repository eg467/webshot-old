using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshot.Forms
{
    public class ProjectInput
    {
        public List<Uri> SpiderSeedUris { get; set; } = new List<Uri>();

        /// <summary>
        /// Which URIs should have generated screenshots.
        /// </summary>
        public List<Uri> SelectedUris { get; set; } = new List<Uri>();
    }
}