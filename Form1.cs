using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Webshot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private readonly Dictionary<int, string> _sizes = new Dictionary<int, string>()
        {
            [480] = "mobile",
            [720] = "tablet",
            [1920] = "desktop"
        };

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var ss = new Screenshotter())
            {
                var outputDir = "screenshots";
                var urls = new Spider("https://example.com").Crawl();
                foreach (var url in urls)
                {
                    foreach (var size in _sizes)
                    {
                        var width = size.Key;
                        var deviceDir = size.Value;
                        var filename = Screenshotter.SanitizeFilename(url.ToString());
                        var path = Path.Combine(outputDir, deviceDir, filename);
                        ss.TakeScreenshot(url.ToString(), path, width);
                    }
                }
            }
            MessageBox.Show("Done");
        }

        private void TakeDeviceScreenshots(string url)
        {
        }
    }

    public enum VisitationStatus
    {
        Visited, Unvisited, Excluded
    }

    public sealed class Spider
    {
        public Uri RootUri { get; }
        private readonly bool _followExternalLinks;
        private readonly Dictionary<Uri, VisitationStatus> _uris = new Dictionary<Uri, VisitationStatus>();

        private IEnumerable<Uri> VisitedUrls => _uris
                .Where(x => x.Value == VisitationStatus.Visited)
                .Select(x => x.Key)
                .OrderBy(x => x);

        public Spider(string rootUri, bool followExternalLinks = false) : this(new Uri(rootUri), followExternalLinks)
        {
        }

        public Spider(Uri rootUri, bool followExternalLinks = false)
        {
            this.RootUri = rootUri;
            this._followExternalLinks = followExternalLinks;
        }

        public IEnumerable<Uri> Crawl()
        {
            _uris.Clear();
            FoundLink(RootUri);

            while (TryNextUnvisited(out Uri unvisited))
            {
                Visit(unvisited);
            }

            return VisitedUrls;
        }

        private void Visit(Uri uri)
        {
            _uris[uri] = VisitationStatus.Visited;

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
                return MatchesHost(uri) && !PermittedUriPattern(uri);
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
            if (!ShouldVisit(uri))
            {
                Exclude(uri);
            }
            else if (!_uris.ContainsKey(uri))
            {
                _uris[uri] = VisitationStatus.Unvisited;
            }
        }

        private void Exclude(Uri uri)
        {
            _uris[uri] = VisitationStatus.Excluded;
        }
    }

    public sealed class Screenshotter : IDisposable
    {
        private readonly ChromeDriver _driver;

        public Screenshotter()
        {
            _driver = CreateDriver();
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }

        private ChromeDriver CreateDriver()
        {
            var driver = new ChromeDriver();
            return driver;
        }

        public static string SanitizeFilename(string filename)
        {
            var sanitized = System.Text.RegularExpressions.Regex.Replace(filename, "[^a-zA-Z0-9]+", "_");
            sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, "__+", "_");
            return sanitized;
        }

        // Adapted from https://stackoverflow.com/a/56535317
        public string TakeScreenshot(string url, string filePath, int? width = null)
        {
            if (!filePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                filePath += ".png";
            }
            string outputDir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            return TakeDriverScreenshot(url, filePath, width);
        }

        public string TakeScreenshot(string url, int? width = null)
        {
            var filePath = GetFilePathFromUri(url);
            return TakeScreenshot(url, filePath, width);
        }

        /// <summary>
        /// Saves a screenshot from a URL. Adapted from https://stackoverflow.com/a/56535317.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private string TakeDriverScreenshot(string url, string filePath, int? width = null)
        {
            _driver.Navigate().GoToUrl(url);
            const string autoWidth = "return Math.max(window.innerWidth,document.body.scrollWidth,document.documentElement.scrollWidth)";
            var calculatedWidth = width.HasValue ? $"return {width}" : autoWidth;

            Dictionary<string, Object> metrics = new Dictionary<string, Object>
            {
                ["width"] = _driver.ExecuteScript(calculatedWidth),
                ["height"] = _driver.ExecuteScript("return Math.max(window.innerHeight,document.body.scrollHeight,document.documentElement.scrollHeight)"),
                ["deviceScaleFactor"] = (double)_driver.ExecuteScript("return window.devicePixelRatio"),
                ["mobile"] = _driver.ExecuteScript("return typeof window.orientation !== 'undefined'")
            };
            _driver.ExecuteChromeCommand("Emulation.setDeviceMetricsOverride", metrics);
            _driver.GetScreenshot().SaveAsFile(filePath, ScreenshotImageFormat.Png);
            _driver.ExecuteChromeCommand("Emulation.clearDeviceMetricsOverride", new Dictionary<string, Object>());
            return filePath;
        }

        private static string GetFilePathFromUri(string websiteUri)
        {
            var uri = new Uri(websiteUri);
            return SanitizeFilename($"{uri.Host}{uri.PathAndQuery}");
        }
    }
}