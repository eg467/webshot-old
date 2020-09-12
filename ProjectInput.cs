using System;
using System.Collections.Generic;

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