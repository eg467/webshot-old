using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;

namespace Webshot
{
    [Serializable]
    public class CrawlResults
    {
        public Dictionary<Uri, VisitationStatus> Uris { get; set; } 
            = new Dictionary<Uri, VisitationStatus>();

        public Dictionary<Uri, BrokenLink> BrokenLinks { get; set; }
            = new Dictionary<Uri, BrokenLink>();
        
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public CrawlResults(Dictionary<Uri, VisitationStatus> uris, Dictionary<Uri, BrokenLink> brokenLinks)
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