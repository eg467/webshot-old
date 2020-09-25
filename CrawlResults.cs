using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Webshot
{
    [Serializable]
    public class CrawlResults
    {
        public Dictionary<Uri, SpiderPageStatus> Uris { get; set; }
            = new Dictionary<Uri, SpiderPageStatus>();

        public List<BrokenLink> BrokenLinks { get; set; } = new List<BrokenLink>();

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public CrawlResults(Dictionary<Uri, SpiderPageStatus> uris, List<BrokenLink> brokenLinks)
        {
            this.Uris = uris;
            this.BrokenLinks = brokenLinks;
            Timestamp = DateTime.Now;
        }

        public CrawlResults()
        {
        }

        public IEnumerable<Uri> SitePages => UrisByStatus(SpiderPageStatus.Visited);

        private IEnumerable<Uri> UrisByStatus(SpiderPageStatus status) =>
            Uris
            .Where(x => x.Value == status)
            .Select(x => x.Key)
            .OrderBy(x => x.ToString());
    }
}