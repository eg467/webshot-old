using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Webshot
{
    public sealed partial class Spider
    {
        private const bool AppendTrailingSlash = true;

        private readonly Dictionary<Uri, VisitationStatus> _uris = new Dictionary<Uri, VisitationStatus>();

        /// <summary>
        /// Keeps track of all link hrefs that point to a given URI.
        /// (For better tracking of broken links.)
        /// </summary>
        private readonly Dictionary<Uri, HashSet<LinkSource>> _linkSources =
            new Dictionary<Uri, HashSet<LinkSource>>();

        private readonly Dictionary<Uri, BrokenLink> _brokenLinks = new Dictionary<Uri, BrokenLink>();

        private readonly List<Uri> _seedUris;
        private readonly SpiderOptions _options;

        public Spider(List<Uri> seedUris, SpiderOptions options)
        {
            this._seedUris = seedUris;
            this._options = options;
        }

        public CrawlResults Crawl(
            IProgress<ParsingProgress> progress = null)
        {
            return Crawl(CancellationToken.None, progress);
        }

        public CrawlResults Crawl(
            CancellationToken token,
            IProgress<ParsingProgress> progress = null)
        {
            _linkSources.Clear();
            _uris.Clear();
            _brokenLinks.Clear();

            _seedUris.ForEach(FoundLink);

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

            return new CrawlResults(_uris, _brokenLinks);
        }

        private void Visit(Uri uri)
        {
            _uris[uri] = VisitationStatus.Visited;

            // Don't recursively follow external links.
            // This will explode the URL list's size.
            if (!IsTrackedHost(uri))
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
                        .Select(h => CreateUri(uri, h.Groups[1].Value))
                        .ToList()
                        .ForEach(FoundLink);
                }
                catch (WebException ex)
                {
                    RecordBrokenLink(uri, ex);
                    Debug.WriteLine($"Error downloading page ({uri}): {ex.Message}");
                    Exclude(uri);
                }
            }
        }

        private void RecordBrokenLink(Uri uri, WebException ex)
        {
            if (!_linkSources.TryGetValue(uri, out var sources))
            {
                sources = new HashSet<LinkSource>();
            }

            _brokenLinks[uri] = new BrokenLink
            {
                ExceptionMessage = $"{ex.Message} ({ex.Status})",
                Uri = uri,
                Sources = sources,
            };
        }

        private bool TrackedScheme(string scheme)
        {
            var validSchemes = new[] { Uri.UriSchemeHttp, Uri.UriSchemeHttps };
            return validSchemes.Contains(scheme);
        }

        private bool ShouldVisit(Uri uri)
        {
            if (uri == null || !TrackedScheme(uri.Scheme))
            {
                return false;
            }

            try
            {
                return IsTrackedHost(uri)
                    && HardCodedValidUri(uri)
                    && ProjectValidUri(uri);
            }
            catch (UriFormatException e)
            {
                Debug.WriteLine($"Error parsing URI ({uri}): {e.Message}");
                return false;
            }
        }

        private bool IsTrackedHost(Uri uri) =>
            !uri.IsAbsoluteUri
            || _options.FollowExternalLinks
            || _seedUris.Any(u => string.Equals(uri.Host, u.Host, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Match hard-coded pattern that exludes URIs of non-html pages.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private bool HardCodedValidUri(Uri uri) =>
            !Regex.IsMatch(uri.ToString(), @"(\.(css|png)$)");

        /// <summary>
        /// Match project settings
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private bool ProjectValidUri(Uri uri) =>
            string.IsNullOrEmpty(_options.UriBlacklistPattern)
            || !Regex.IsMatch(uri.ToString(), _options.UriBlacklistPattern);

        private bool TryNextUnvisited(out Uri uri)
        {
            uri = _uris
                .Where(x => x.Value == VisitationStatus.Unvisited)
                .Select(x => x.Key)
                .FirstOrDefault();
            return uri != null;
        }

        private Uri CreateUri(Uri callingPage, string href)
        {
            // Record all hrefs associated with this Uri
            var uri = new Uri(callingPage, href);
            RecordLinkSource(callingPage, uri, href);
            return uri;
        }

        private void RecordLinkSource(Uri callingPage, Uri target, string linkHref)
        {
            callingPage = callingPage.TryStandardize();
            target = target.TryStandardize();
            if (!_linkSources.TryGetValue(target, out var sources))
            {
                sources = new HashSet<LinkSource>();
                _linkSources[target] = sources;
            }
            var source = new LinkSource
            {
                Href = linkHref,
                CallingPage = callingPage
            };
            sources.Add(source);
        }

        private void FoundLink(Uri uri)
        {
            Uri standardizedUri = uri.TryStandardize();
            if (!ShouldVisit(standardizedUri))
            {
                Exclude(standardizedUri);
            }
            else if (!_uris.ContainsKey(standardizedUri))
            {
                VisitLater(standardizedUri);
            }
        }

        private void Exclude(Uri uri)
        {
            _uris[uri] = VisitationStatus.Excluded;
        }

        private void VisitLater(Uri uri)
        {
            _uris[uri] = VisitationStatus.Unvisited;
        }
    }

    public class ParsingProgress
    {
        public int CurrentIndex { get; set; }
        public int Count { get; set; }
        public string CurrentItem { get; set; }
    }

    public class BrokenLink
    {
        public string ExceptionMessage { get; set; }
        public Uri Uri { get; set; }
        public HashSet<LinkSource> Sources { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is BrokenLink link))
            {
                return false;
            }
            return Uri.Equals(link.Uri);
        }

        public override int GetHashCode() => Uri.GetHashCode();
    }

    public class LinkSource
    {
        /// <summary>
        /// The page that references the broken link.
        /// </summary>
        public Uri CallingPage { get; set; }

        /// <summary>
        /// The raw link in the anchor tag for finding the broken link sources.
        /// </summary>
        public string Href { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is LinkSource src))
            {
                return false;
            }
            return CallingPage.Equals(src.CallingPage)
                && string.Equals(Href, src.Href, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode() =>
            Utils.CombineHashCodes(CallingPage.GetHashCode(), Href.GetHashCode());
    }

    public enum VisitationStatus
    {
        Visited, Unvisited, Excluded
    }
}