using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Webshot
{
    public static class WebshotHttpClient
    {
        private static readonly HttpClientHandler HttpClientHandler = new HttpClientHandler();
        public static HttpClient Client { get; } = new HttpClient(HttpClientHandler);

        static WebshotHttpClient()
        {
            Client.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(nameof(Webshot), Application.ProductVersion));
        }

        public static void SetCredentials(ICredentials credentials)
        {
            HttpClientHandler.Credentials = credentials;
        }
    }

    public sealed class Spider : IDisposable
    {
        private readonly LinkTracker _linkTracker = new LinkTracker();
        private readonly List<Uri> _seedUris;
        private readonly SpiderOptions _options;
        private readonly ICredentials _credentials;

        public Spider(List<Uri> seedUris, SpiderOptions options, ProjectCredentials creds)
        {
            this._seedUris = seedUris;
            this._options = options;
            SetHttpClientCredentials(creds);
        }

        private void SetHttpClientCredentials(ProjectCredentials projectCreds)
        {
            var myCache = new CredentialCache();
            projectCreds?.CredentialsByDomain?.ForEach(c =>
            {
                var domainUrl =
                    c.Key.Contains(Uri.SchemeDelimiter)
                    ? c.Key : $"{Uri.UriSchemeHttps}{Uri.SchemeDelimiter}{c.Key}";
                var uri = new Uri(domainUrl);
                var creds = new NetworkCredential(c.Value.User, c.Value.Password);

                myCache.Add(uri, "Basic", creds);
            });
            WebshotHttpClient.SetCredentials(myCache);
        }

        public async Task<CrawlResults> Crawl(
            IProgress<TaskProgress> progress = null)
        {
            return await Crawl(CancellationToken.None, progress);
        }

        public async Task<CrawlResults> Crawl(
            CancellationToken token,
            IProgress<TaskProgress> progress = null)
        {
            _linkTracker.Clear();
            _seedUris.Select(x => new Link(x, "")).ForEach(FoundLink);

            while (_linkTracker.TryNextUnvisited(out StandardizedUri unvisited))
            {
                Debug.WriteLine($"Parsing {unvisited}");

                if (token.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }

                progress?.Report(new TaskProgress
                {
                    CurrentItem = unvisited.Standardized.ToString(),
                    Count = _linkTracker.Count(),
                    CurrentIndex = _linkTracker.Count(u => u.Status != VisitationStatus.Unvisited)
                });

                await Visit(unvisited);
            }

            return _linkTracker.ToCrawlResults();
        }

        private async Task Visit(StandardizedUri uri)
        {
            UriSources sources = null;
            string html = "";
            try
            {
                (Uri finalUri, string content) = await DownloadPageAsync(uri.Standardized);
                html = content;
                sources = _linkTracker.Redirect(uri, finalUri);
            }
            catch (HttpRequestException ex)
            {
                sources = _linkTracker.GetOrCreateSources(uri);
                sources.Status = VisitationStatus.Error;
                sources.Error = ex.Message;
                Debug.WriteLine($"Error downloading page ({uri}): {ex.Message}");
            }

            if (sources.Status != VisitationStatus.Unvisited)
            {
                return;
            }

            if (!ContentValid(html))
            {
                sources.Status = VisitationStatus.Excluded;
                return;
            }

            sources.Status = VisitationStatus.Visited;
            ParseLinks(sources.Uri.Uri, html)
                .ForEach(FoundLink);
        }

        private static IEnumerable<Link> ParseLinks(Uri callingPage, string html) =>
            Regex.Matches(html, @"<a[^>]+href=""([^""]+)""", RegexOptions.IgnoreCase)
                .Cast<Match>()
                .Select(h => new Link(callingPage, h.Groups[1].Value));

        /// <summary>
        /// Downloads the contents of a web page
        /// </summary>
        /// <param name="uri">The URI of the page to download</param>
        /// <returns>A tuple of the Uri of the final page (after redirection), and its HTML contents.</returns>
        private async Task<Tuple<Uri, string>> DownloadPageAsync(Uri uri)
        {
            using (HttpResponseMessage response = await WebshotHttpClient.Client.GetAsync(uri))
            {
                var finalUri = response.RequestMessage.RequestUri;
                response.EnsureSuccessStatusCode();
                using (HttpContent content = response.Content)
                {
                    string result = await content.ReadAsStringAsync();
                    return new Tuple<Uri, string>(finalUri, result);
                }
            }
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
        private bool HardCodedValidUri(Uri uri)
        {
            // Check if last three path segments repeat
            // because WordPress can allow infinite recursion with poorly formed links.
            // e.g. https://example.com/page/page/page/.../

            // The maximum allowable number of repetitions at the end of the path.
            var maxRecursionLevels = 2;

            var recursionCheckDesired = maxRecursionLevels >= 1;
            var illegalRecursionPossible = uri.Segments.Length > maxRecursionLevels + 1;

            var hasRecursed =
                recursionCheckDesired
                && illegalRecursionPossible
                && uri.Segments
                    .Skip(uri.Segments.Length - maxRecursionLevels)
                    .Select(s => s.TrimEnd('/'))
                    .Unanimous();

            var excludedProtocols = "tel|mailto";
            var excludedExtensions = "css|png|pdf";

            var isBlacklisted = Regex.IsMatch(
                uri.ToString(),
                $@"({excludedProtocols}|\.({excludedExtensions})$)");

            return !hasRecursed && !isBlacklisted;
        }

        /// <summary>
        /// Match project settings
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private bool ProjectValidUri(Uri uri) =>
            string.IsNullOrEmpty(_options.UriBlacklistPattern)
            || !Regex.IsMatch(uri.ToString(), _options.UriBlacklistPattern);

        private static bool ContentValid(string content)
        {
            return content.IndexOf("<html", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void FoundLink(Link link)
        {
            var sources = _linkTracker.GetOrCreateSources(link.Target);
            sources.CallingLinks.Add(link);

            if (sources.Status != VisitationStatus.Excluded
                && !ShouldVisit(sources.Uri.Standardized))
            {
                sources.Status = VisitationStatus.Excluded;
            }
        }

        public void Dispose()
        {
            WebshotHttpClient.SetCredentials(null);
        }
    }

    public class TaskProgress
    {
        public int CurrentIndex { get; set; }
        public int Count { get; set; }
        public string CurrentItem { get; set; }
    }

    public class Link
    {
        /// <summary>
        /// The page that references the broken link.
        /// </summary>
        public Uri CallingPage { get; }

        /// <summary>
        /// The raw link in the anchor tag for finding the broken link sources.
        /// </summary>
        public string Href { get; }

        public Uri Target => new Uri(CallingPage, Href);

        public Link(Uri callingPage, string href)
        {
            this.CallingPage = callingPage;
            this.Href = href;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Link src))
            {
                return false;
            }
            return CallingPage.Equals(src.CallingPage)
                && string.Equals(Href, src.Href, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode() =>
            Utils.CombineHashCodes(CallingPage.GetHashCode(), Href.GetHashCode());
    }

    public class StandardizedUri
    {
        public Uri Uri { get; }
        public Uri Standardized { get; }

        public StandardizedUri(Uri uri)
        {
            Uri = uri;
            Standardized = uri.TryStandardize();
        }

        public override bool Equals(object obj) =>
            Equals((obj as StandardizedUri)?.Standardized, Standardized);

        public override int GetHashCode() => Standardized.GetHashCode();
    }

    public enum VisitationStatus
    {
        Visited, Unvisited, Excluded, Redirected, Error
    }

    public class UriSources
    {
        public VisitationStatus Status { get; set; } = VisitationStatus.Unvisited;
        public StandardizedUri Uri { get; set; }
        public Uri RedirectedUri { get; set; }

        public bool WasRedirected =>
            RedirectedUri != null
            && Equals(Uri, new StandardizedUri(RedirectedUri));

        public string Error { get; set; }
        public HashSet<Link> CallingLinks { get; set; } = new HashSet<Link>();

        public UriSources(StandardizedUri uri)
        {
            this.Uri = uri;
        }
    }

    public class BrokenLink
    {
        public Uri Target { get; set; }
        public HashSet<Link> Sources { get; set; }
        public string Error { get; set; }
    }

    public class LinkTracker
    {
        private readonly Dictionary<StandardizedUri, UriSources> _uris =
            new Dictionary<StandardizedUri, UriSources>();

        public int Count(Func<UriSources, bool> predicate = null) =>
            predicate != null
            ? _uris.Count(x => predicate(x.Value))
            : _uris.Count();

        public bool Contains(StandardizedUri uri) => _uris.ContainsKey(uri);

        public void Clear()
        {
            _uris.Clear();
        }

        public CrawlResults ToCrawlResults() => new CrawlResults(ByStatus, BrokenLinks);

        public List<BrokenLink> BrokenLinks =>
            _uris
            .Where(x => x.Value.Status == VisitationStatus.Error)
            .Select(x => new BrokenLink()
            {
                Error = x.Value.Error,
                Target = x.Key.Standardized,
                Sources = x.Value.CallingLinks
            })
            .ToList();

        public Dictionary<Uri, VisitationStatus> ByStatus =>
            _uris
            .ToDictionary(x => x.Key.Standardized, x => x.Value.Status);

        public List<Uri> AvailableWebPages() =>
            _uris
            .Where(x => x.Value.Status == VisitationStatus.Visited)
            .Select(x => x.Key.Standardized)
            .ToList();

        public UriSources GetOrCreateSources(Uri uri) =>
            GetOrCreateSources(new StandardizedUri(uri));

        public UriSources GetOrCreateSources(StandardizedUri uri)
        {
            if (!_uris.TryGetValue(uri, out var sources))
            {
                sources = new UriSources(uri);
                _uris[uri] = sources;
            }
            return sources;
        }

        public UriSources Redirect(StandardizedUri sourceUri, Uri redirectionTarget)
        {
            var src = GetOrCreateSources(sourceUri);
            var stdDestUri = new StandardizedUri(redirectionTarget);
            src.RedirectedUri = redirectionTarget;

            if (sourceUri.Equals(stdDestUri))
            {
                // Not a meaningful redirection
                return src;
            }

            src.Status = VisitationStatus.Redirected;

            var dest = GetOrCreateSources(stdDestUri);
            var combinedLinks = src.CallingLinks.Union(dest.CallingLinks).ToHashSet();
            src.CallingLinks = combinedLinks;
            dest.CallingLinks = combinedLinks;

            return dest;
        }

        public bool TryNextUnvisited(out StandardizedUri uri)
        {
            uri = _uris
                .Where(x => x.Value.Status == VisitationStatus.Unvisited)
                .Select(x => x.Key)
                .FirstOrDefault();
            return uri != null;
        }
    }
}