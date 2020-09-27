using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        // This can't be edited after sending a request
        public static CredentialCache Credentials
        {
            get => HttpClientHandler.Credentials as CredentialCache;
            set => HttpClientHandler.Credentials = value;
        }

        public static readonly HttpClient Client = new HttpClient(HttpClientHandler);

        private static readonly CredentialCache _credentialCache = new CredentialCache();

        static WebshotHttpClient()
        {
            HttpClientHandler.Credentials = _credentialCache;

            // Identify as a bot user agent
            Client.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(nameof(Webshot), Application.ProductVersion));
        }

        public static void AddCredential(Uri host, NetworkCredential credential)
        {
            if (_credentialCache.GetCredential(host, "Basic") is null)
            {
                _credentialCache.Add(host, "Basic", credential);
            }
        }
    }

    public sealed class Spider : IDisposable
    {
        private readonly LinkTracker _linkTracker = new LinkTracker();
        private readonly List<Uri> _seedUris;
        private readonly SpiderOptions _options;

        public Spider(List<Uri> seedUris, SpiderOptions options, ProjectCredentials creds)
        {
            _seedUris = seedUris;
            _options = options;

            SetHttpClientCredentials(creds);
            ConfigureUriCrawlValidators();
        }

        //private CredentialCache _credentialCache;

        private void SetHttpClientCredentials(ProjectCredentials projectCreds)
        {
            //var credCache = new CredentialCache();
            projectCreds?.CredentialsByDomain?.ForEach(c =>
            {
                var domainUrl = c.Key.Contains("://") ? c.Key : $"https://{c.Key}";
                var uri = new Uri(domainUrl);
                var creds = new NetworkCredential(c.Value.DecryptUser(), c.Value.DecryptPassword());

                WebshotHttpClient.AddCredential(uri, creds);
                //_credentialCache.Add(uri, "Basic", creds);
            });

            //WebshotHttpClient.Credentials = credCache;
        }

        public async Task<CrawlResults> Crawl(IProgress<TaskProgress> progress = null) =>
            await Crawl(CancellationToken.None, progress);

        public async Task<CrawlResults> Crawl(
            CancellationToken token,
            IProgress<TaskProgress> progress = null)
        {
            _linkTracker.Clear();
            _seedUris.Select(x => new Link(x, "")).ForEach(FoundLink);

            while (_linkTracker.TryNextUnvisited(out StandardizedUri unvisited))
            {
                Debug.WriteLine($"Parsing {unvisited.Standardized.AbsoluteUri}");

                if (token.IsCancellationRequested)
                {
                    throw new TaskCanceledException();
                }

                var currentProgress = new TaskProgress(
                    _linkTracker.Count(u => u.Status != SpiderPageStatus.Unvisited),
                    _linkTracker.Count(),
                    unvisited.Standardized.ToString());

                progress?.Report(currentProgress);

                await Visit(unvisited);
            }

            TaskProgress completionProgress = new TaskProgress(_linkTracker.Count(), _linkTracker.Count(), "Complete");
            progress?.Report(completionProgress);

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
                sources = _linkTracker.CombineIfRedirection(uri, finalUri);
            }
            catch (HttpRequestException ex)
            {
                sources = _linkTracker.GetOrCreateSources(uri);
                sources.Status = SpiderPageStatus.Error;
                sources.Error = ex.Message;
                Debug.WriteLine($"Error downloading page ({uri}): {ex.Message}");
            }

            if (sources.Status != SpiderPageStatus.Unvisited) return;

            if (!IsHtml(html))
            {
                sources.Status = SpiderPageStatus.Excluded;
                return;
            }

            sources.Status = SpiderPageStatus.Visited;

            if (_options.FollowLinks)
            {
                ParseLinks(sources.Uri.Uri, html).ForEach(FoundLink);
            }
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
        private async Task<(Uri finalPage, string contents)> DownloadPageAsync(Uri uri)
        {
            using (HttpResponseMessage response = await WebshotHttpClient.Client.GetAsync(uri))
            {
                response.EnsureSuccessStatusCode();
                using (HttpContent content = response.Content)
                {
                    var finalUri = response.RequestMessage.RequestUri;
                    string result = await content.ReadAsStringAsync();
                    return (finalUri, result);
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
            try
            {
                return ValidateUri(uri);
            }
            catch (UriFormatException e)
            {
                Debug.WriteLine($"Error parsing URI ({uri}): {e.Message}");
                return false;
            }
        }

        private readonly List<UriCrawlValidator> _uriCrawlValidators = new List<UriCrawlValidator>();

        private void ConfigureUriCrawlValidators()
        {
            _uriCrawlValidators.Clear();
            Action<UriCrawlValidator> Add = _uriCrawlValidators.Add;
            if (!_options.TrackExternalLinks)
            {
                Add(new UriInTrackedDomainValidator(_seedUris));
            }

            if (!string.IsNullOrWhiteSpace(_options.UriBlacklistPattern))
            {
                Add(new UriRegexValidator(_options.UriBlacklistPattern, UriCrawlValidatorType.Blacklist));
            }

            Add(new UriRecursionValidator());
            Add(new PermittedUriSchemeValidator("http", "https"));
            Add(new ForbiddenUriExtensionValidator("css", "png", "jpg", "jpeg", "js", "pdf"));
        }

        private bool ValidateUri(Uri uri) =>
            !string.IsNullOrEmpty(uri?.AbsoluteUri)
            && _uriCrawlValidators.All(v => v.Validate(uri));

        private static bool IsHtml(string content) =>
            content.IndexOf("<html", StringComparison.OrdinalIgnoreCase) >= 0;

        private void FoundLink(Link link)
        {
            var sources = _linkTracker.GetOrCreateSources(link.Target);
            sources.CallingLinks.Add(link);

            if (sources.Status != SpiderPageStatus.Excluded
                && !ShouldVisit(sources.Uri.Standardized))
            {
                sources.Status = SpiderPageStatus.Excluded;
            }
        }

        public void Dispose()
        {
            WebshotHttpClient.Credentials = null;
        }
    }

    /// <summary>
    /// Determines if a url should be validated.
    /// </summary>
    internal abstract class UriCrawlValidator
    {
        public abstract bool Validate(Uri uri);
    }

    internal class UriInTrackedDomainValidator : UriCrawlValidator
    {
        private readonly List<Uri> _seedUris;

        public UriInTrackedDomainValidator(List<Uri> seedUris)
        {
            _seedUris = seedUris;
        }

        public override bool Validate(Uri uri)
        {
            bool MatchesHost(Uri u) =>
                string.Equals(uri.Host, u.Host, StringComparison.OrdinalIgnoreCase);
            return !uri.IsAbsoluteUri || _seedUris.Any(MatchesHost);
        }
    }

    /// <summary>
    /// Check if last N path segments repeat
    /// because WordPress can generate infinite recursion with poorly formed links.
    /// e.g. https://example.com/page/page/page/.../
    /// </summary>
    internal class UriRecursionValidator : UriCrawlValidator
    {
        private readonly int _maxRecursionLevel;

        public UriRecursionValidator(int maxRecursionLevel = 2)
        {
            _maxRecursionLevel = maxRecursionLevel;
        }

        public override bool Validate(Uri uri)
        {
            var recursionCheckDesired = _maxRecursionLevel >= 1;
            var illegalRecursionPossible = uri.Segments.Length > _maxRecursionLevel + 1;

            var hasRecursed =
                recursionCheckDesired
                && illegalRecursionPossible
                && uri.Segments
                    .Skip(uri.Segments.Length - _maxRecursionLevel)
                    .Select(s => s.TrimEnd('/'))
                    .Unanimous();
            return !hasRecursed;
        }
    }

    internal enum UriCrawlValidatorType { Whitelist, Blacklist }

    internal class UriRegexValidator : UriCrawlValidator
    {
        private readonly string _pattern;
        private readonly bool _caseSensitive;

        /// <summary>
        /// True to include the matched pattern, false to reject a matching pattern.
        /// </summary>
        private readonly UriCrawlValidatorType _isWhitelist;

        public UriRegexValidator(string pattern, UriCrawlValidatorType isWhitelist = UriCrawlValidatorType.Whitelist, bool caseSensitive = false)
        {
            _pattern = pattern;
            _caseSensitive = caseSensitive;
            _isWhitelist = isWhitelist;
        }

        public override bool Validate(Uri uri)
        {
            var options = _caseSensitive ? RegexOptions.IgnoreCase : RegexOptions.None;
            var isMatch = Regex.IsMatch(uri.AbsoluteUri, _pattern, options);
            return _isWhitelist == UriCrawlValidatorType.Whitelist ? isMatch : !isMatch;
        }
    }

    internal class ForbiddenUriExtensionValidator : UriRegexValidator
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="extensionBlacklist">Forbidden URI extensions, not including periods.</param>
        public ForbiddenUriExtensionValidator(params string[] extensionBlacklist)
            : base($@"\.({string.Join("|", extensionBlacklist)})\b", UriCrawlValidatorType.Blacklist)
        {
        }
    }

    internal class PermittedUriSchemeValidator : UriRegexValidator
    {
        public PermittedUriSchemeValidator(params string[] schemeWhitelist)
            : base($@"^{string.Join("|", schemeWhitelist)}:", UriCrawlValidatorType.Whitelist)
        {
        }
    }

    public class TaskProgress
    {
        public int CurrentIndex { get; set; }
        public int Count { get; set; }
        public string CurrentItem { get; set; }

        public TaskProgress(int currentIndex, int count, string currentItem)
        {
            this.CurrentIndex = currentIndex;
            this.Count = count;
            this.CurrentItem = currentItem;
        }
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

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SpiderPageStatus
    {
        Visited, Unvisited, Excluded, Redirected, Error
    }

    public class UriSources
    {
        public SpiderPageStatus Status { get; set; } = SpiderPageStatus.Unvisited;
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
            .Where(x => x.Value.Status == SpiderPageStatus.Error)
            .Select(x => new BrokenLink()
            {
                Error = x.Value.Error,
                Target = x.Key.Standardized,
                Sources = x.Value.CallingLinks
            })
            .ToList();

        public Dictionary<Uri, SpiderPageStatus> ByStatus =>
            _uris
            .ToDictionary(x => x.Key.Standardized, x => x.Value.Status);

        public List<Uri> AvailableWebPages() =>
            _uris
            .Where(x => x.Value.Status == SpiderPageStatus.Visited)
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

        public UriSources CombineIfRedirection(StandardizedUri sourceUri, Uri redirectionTarget)
        {
            var src = GetOrCreateSources(sourceUri);
            var stdDestUri = new StandardizedUri(redirectionTarget);
            src.RedirectedUri = redirectionTarget;

            if (sourceUri.Equals(stdDestUri))
            {
                // Not a meaningful redirection
                return src;
            }

            // The request has been redirected,
            // so the source and destination pages should be considered equivalent.
            // Combine the pages that point to either.
            src.Status = SpiderPageStatus.Redirected;

            var dest = GetOrCreateSources(stdDestUri);
            var combinedLinks = src.CallingLinks.Union(dest.CallingLinks).ToHashSet();
            src.CallingLinks = combinedLinks;
            dest.CallingLinks = combinedLinks;

            return dest;
        }

        public bool TryNextUnvisited(out StandardizedUri uri)
        {
            uri = _uris
                .Where(x => x.Value.Status == SpiderPageStatus.Unvisited)
                .Select(x => x.Key)
                .FirstOrDefault();
            return uri is object;
        }
    }
}