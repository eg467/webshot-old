using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Webshot
{
    [Serializable]
    public class CrawlResults
    {
        public Dictionary<Uri, VisitationStatus> Uris { get; set; }
            = new Dictionary<Uri, VisitationStatus>();

        public List<BrokenLink> BrokenLinks { get; set; } = new List<BrokenLink>();

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public CrawlResults(Dictionary<Uri, VisitationStatus> uris, List<BrokenLink> brokenLinks)
        {
            this.Uris = uris;
            this.BrokenLinks = brokenLinks;
            Timestamp = DateTime.Now;
        }

        public CrawlResults()
        {
        }

        public IEnumerable<Uri> SitePages => UrisByStatus(VisitationStatus.Visited);

        private IEnumerable<Uri> UrisByStatus(VisitationStatus status) =>
            Uris
            .Where(x => x.Value == status)
            .Select(x => x.Key)
            .OrderBy(x => x.ToString());
    }
}