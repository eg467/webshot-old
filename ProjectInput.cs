using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Webshot.Forms
{
    public class ProjectInput
    {
        public List<Uri> SpiderSeedUris { get; set; } = new List<Uri>();

        /// <summary>
        /// A lsit of all the site page URIs and whether they should be screenshotted.
        /// </summary>
        public List<(Uri uri, bool selected)> SiteUris { get; set; } =
            new List<(Uri uri, bool selected)>();

        /// <summary>
        /// Which URIs should have generated screenshots.
        /// </summary>
        [JsonIgnore]
        public List<Uri> SelectedPages => SiteUris
            .Where(x => x.selected)
            .Select(x => x.uri)
            .ToList();
    }
}