using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Webshot
{
    public sealed class Spider
    {
        private const bool AppendTrailingSlash = true;

        public class CrawlResults
        {
            private readonly Dictionary<Uri, VisitationStatus> _uris;

            public CrawlResults(Dictionary<Uri, VisitationStatus> uris)
            {
                _uris = uris;
            }

            public IEnumerable<Uri> VisitedUrls => _uris
                .Where(x => x.Value == VisitationStatus.Visited)
                .Select(x => x.Key)
                .OrderBy(x => x.ToString());

            public IEnumerable<Uri> ExcludedUrls => _uris
                .Where(x => x.Value == VisitationStatus.Excluded)
                .Select(x => x.Key)
                .OrderBy(x => x.ToString());
        }

        public Uri RootUri { get; }
        private readonly bool _followExternalLinks;
        private readonly Dictionary<Uri, VisitationStatus> _uris = new Dictionary<Uri, VisitationStatus>();

        public Spider(string rootUri, bool followExternalLinks = false) : this(new Uri(rootUri), followExternalLinks)
        {
        }

        public Spider(Uri rootUri, bool followExternalLinks = false)
        {
            this.RootUri = rootUri;
            this._followExternalLinks = followExternalLinks;
        }

        public CrawlResults Crawl(IProgress<ParsingProgress> progress = null)
        {
            return Crawl(CancellationToken.None, progress);
        }

        public CrawlResults Crawl(CancellationToken token, IProgress<ParsingProgress> progress = null)
        {
            _uris.Clear();
            FoundLink(RootUri);

            while (TryNextUnvisited(out Uri unvisited))
            {
                if (token.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }

                progress?.Report(new ParsingProgress
                {
                    CurrentItem = unvisited.ToString(),
                    Count = _uris.Count,
                    CurrentIndex = _uris.Count(u => u.Value != VisitationStatus.Unvisited)
                });

                Visit(unvisited);

                Application.DoEvents();
            }

            return new CrawlResults(_uris);
        }

        private void Visit(Uri uri)
        {
            _uris[uri] = VisitationStatus.Visited;

            // Don't recursively follow external links.
            // This will explode the URL list's size.
            if (!MatchesHost(uri))
            {
                return;
            }

            using (var client = new WebClient())
            {
                try
                {
                    var html = client.DownloadString(uri);
                    if (html.IndexOf("<html", StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        Exclude(uri);
                        return;
                    }

                    Regex.Matches(html, @"<a[^>]+href=""([^""]+)""", RegexOptions.IgnoreCase)
                        .Cast<Match>()
                        .Select(h => CreateUri(h.Groups[1].Value))
                        .ToList()
                        .ForEach(FoundLink);
                }
                catch (WebException e)
                {
                    Debug.WriteLine($"Error downloading page ({uri}): {e.Message}");
                    Exclude(uri);
                }
            }
        }

        private bool ShouldVisit(Uri uri)
        {
            try
            {
                bool hostMatch = MatchesHost(uri);
                bool validPattern = PermittedUriPattern(uri);
                return hostMatch && validPattern;
            }
            catch (UriFormatException e)
            {
                Debug.WriteLine($"Error parsing URI ({uri}): {e.Message}");
                return false;
            }
        }

        private bool MatchesHost(Uri uri) =>
            !uri.IsAbsoluteUri
            || _followExternalLinks
            || string.Equals(uri.Host, RootUri.Host, StringComparison.OrdinalIgnoreCase);

        private bool PermittedUriPattern(Uri uri) =>
            !Regex.IsMatch(uri.ToString(), @"(\.(css|png)$)");

        private bool TryNextUnvisited(out Uri uri)
        {
            uri = _uris
                .Where(x => x.Value == VisitationStatus.Unvisited)
                .Select(x => x.Key)
                .FirstOrDefault();
            return uri != null;
        }

        private Uri CreateUri(string uri) => new Uri(RootUri, uri);

        private void FoundLink(Uri uri)
        {
            var standardizedUri = StandardizeUri(uri);
            if (!ShouldVisit(standardizedUri))
            {
                Exclude(standardizedUri);
            }
            else if (!_uris.ContainsKey(standardizedUri))
            {
                _uris[standardizedUri] = VisitationStatus.Unvisited;
            }
        }

        private Uri StandardizeUri(Uri uri)
        {
            // Removes fragment/anchor
            // Adds slashes to path if option is enabled and the url isn't a filename with extension
            string lastSegment = uri.Segments.Last();
            bool addSlash = AppendTrailingSlash
                && !lastSegment.Contains(".")
                && !lastSegment.EndsWith("/");

            string addedSlash = addSlash ? "/" : "";
            return new Uri($"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}{addedSlash}{uri.Query}");
        }

        private void Exclude(Uri uri)
        {
            _uris[uri] = VisitationStatus.Excluded;
        }
    }

    public class ParsingProgress
    {
        public int CurrentIndex { get; set; }
        public int Count { get; set; }
        public string CurrentItem { get; set; }
    }
}