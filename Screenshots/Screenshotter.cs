using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Webshot
{
    public sealed class Screenshotter : IDisposable
    {
        private const ScreenshotImageFormat _imageFormat = ScreenshotImageFormat.Png;

        /// <summary>
        /// The image file extension of screenshots, e.g. ".png".
        /// </summary>
        public static readonly string ImageExtension = $".{_imageFormat.ToString().ToLower()}";

        private readonly ChromeDriver _driver;
        private readonly ScreenshotOptions _options;
        private readonly Dictionary<Uri, BrokenLink> _brokenLinks;
        private readonly ProjectCredentials _projectCredentials;

        public Screenshotter(
            ScreenshotOptions options,
            IEnumerable<BrokenLink> brokenLinks,
            ProjectCredentials projectCredentials)
        {
            _options = options ?? new ScreenshotOptions();
            brokenLinks = brokenLinks ?? Enumerable.Empty<BrokenLink>();
            _brokenLinks = brokenLinks.ToDictionary(x => x.Target, x => x);
            _projectCredentials = projectCredentials ?? new ProjectCredentials();
            _driver = CreateDriver();
        }

        public Screenshotter() : this(
            new ScreenshotOptions(),
            new List<BrokenLink>(),
            new ProjectCredentials())
        {
        }

        public Screenshotter(Project project)
            : this(
                project.Options?.ScreenshotOptions,
                project.CrawledPages?.BrokenLinks,
                project.Options?.Credentials)
        {
        }

        public void Dispose()
        {
            if (File.Exists(AuthExtensionPath))
            {
                File.Delete(AuthExtensionPath);
            }
            _driver.Close();
            _driver.Dispose();
        }

        private ChromeDriver CreateDriver()
        {
            var options = new ChromeOptions();
            CreateAuthExtensionIfNeeded(options);
            var driver = new ChromeDriver(options);
            return driver;
        }

        private const string AuthExtensionPath = "authextension.zip";
        private bool _extensionLoaded;

        /// <summary>
        /// <para>Creates an extension that handles basic authentication.</para>
        /// <para>WARNING: this will store usernames and passwords in plaintext</para>
        /// </summary>
        /// <param name="options"></param>
        private void CreateAuthExtensionIfNeeded(ChromeOptions options)
        {
            using (var ext = new ChromeAuthExtension(_projectCredentials, "temp-extension-files"))
            {
                if (!ext.IsNeeded)
                {
                    _extensionLoaded = false;
                    return;
                }

                if (File.Exists(AuthExtensionPath))
                {
                    File.Delete(AuthExtensionPath);
                }
                ext.CreateZip(AuthExtensionPath);
                options.AddArguments("--no-sandbox");
                options.AddExtensions(AuthExtensionPath);
                _extensionLoaded = true;
            }
        }

        /// <summary>
        /// Takes a screenshot of a web page.
        /// </summary>
        /// <param name="url">The URL of the web page to screenshoot.</param>
        /// <param name="filePath">The image file to save to.</param>
        /// <param name="width">The device width of the browser</param>
        /// <returns>The full file path of the saved image</returns>
        public NavigationTiming TakeScreenshot(string url, string filePath, int? width = null)
        {
            var outputDir = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(outputDir);
            _driver.Navigate().GoToUrl(url);

            ResizeWindow();
            if (_options.HighlightBrokenLinks)
            {
                HighlightBrokenLinks(url);
            }

            var requestStats = GetRequestStats();

            int screenshotDelay = 1000;
            Thread.Sleep(screenshotDelay);

            var screenshot = _driver.GetScreenshot();
            screenshot.SaveAsFile(filePath, _imageFormat);
            ClearResize();
            return requestStats;

            // LOCAL FUNCTIONS

            NavigationTiming GetRequestStats()
            {
                //JSON.stringify([...window.performance.getEntriesByType("navigation"),{ }][0])
                try
                {
                    var stats = (string)_driver.ExecuteScript(
                        @"return (window && window.performance && JSON.stringify([...window.performance.getEntriesByType('navigation'),{ }][0])) || '{}'");

                    string ConvertToInt(Match m)
                    {
                        var origValue = m.Value;
                        if (!double.TryParse(origValue, out var dblVal)) return origValue;
                        var intValue = (int)Math.Round(dblVal);
                        return intValue.ToString();
                    }

                    stats = Regex.Replace(stats, @"\d+\.\d+", ConvertToInt);

                    return JsonConvert.DeserializeObject<NavigationTiming>((string)stats);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error parsing request timings: " + ex.Message);
                    return new NavigationTiming();
                }
            }

            /// <summary>
            /// Repeatedly resize the window to account for lazily loaded elements.
            /// </summary>
            /// <param name="width"></param>
            void ResizeWindow()
            {
                // Adapted from https://stackoverflow.com/a/56535317
                int numTries = 0;
                const int maxTries = 8;
                int prevHeight;
                int calculatedHeight = -1;
                string autoWidthCommand =
                        @"return Math.max(
                        window.innerWidth,
                        document.body.scrollWidth,
                        document.documentElement.scrollWidth)";

                // Repeatedly resize height to allow new elements to (lazily) load.
                do
                {
                    prevHeight = calculatedHeight;
                    var calculatedWidth = width.HasValue ? $"return {width}" : autoWidthCommand;
                    calculatedHeight = CalculateDocHeight();

                    // TODO: Set device-specific user agents.
                    Dictionary<string, object> metrics = new Dictionary<string, object>
                    {
                        ["width"] = _driver.ExecuteScript(calculatedWidth),
                        ["height"] = calculatedHeight,
                        ["deviceScaleFactor"] = ScaleFactor(false),
                        ["mobile"] = _driver.ExecuteScript("return typeof window.orientation !== 'undefined'")
                    };
                    _driver.ExecuteChromeCommand("Emulation.setDeviceMetricsOverride", metrics);
                } while (calculatedHeight != prevHeight && ++numTries < maxTries);

                // LOCAL FUNCTIONS

                int CalculateDocHeight()
                {
                    // Sometimes a long, sometimes double, etc
                    object jsHeight = _driver.ExecuteScript(
                        @"return Math.max(
                        document.body.scrollHeight,
                        document.body.offsetHeight,
                        document.documentElement.clientHeight,
                        document.documentElement.scrollHeight,
                        document.documentElement.offsetHeight,
                        document.documentElement.getBoundingClientRect().height)");
                    double numericHeight = Convert.ToDouble(jsHeight);
                    return (int)Math.Ceiling(numericHeight);
                }

                // False for a 1:1 pixel ratio with the image
                // True for an easier-to-read image on the current monitor.
                double ScaleFactor(bool shouldScaleImage) =>
                    shouldScaleImage
                    ? (double)_driver.ExecuteScript("return window.devicePixelRatio")
                    : 1.0;
            }

            void ClearResize()
            {
                _driver.ExecuteChromeCommand("Emulation.clearDeviceMetricsOverride", new Dictionary<string, object>());
            }

            void HighlightBrokenLinks(string rawCallingUri)
            {
                var standardizedUri = new Uri(rawCallingUri);
                standardizedUri = standardizedUri.TryStandardize();

                if (_brokenLinks?.Any() != true) return;

                var selectors =
                    _brokenLinks
                        .SelectMany(l => l.Value.Sources)
                        .Where(x => standardizedUri.Equals(x.CallingPage))
                        .Select(x => $@"a[href='{x.Href}']");

                var combinedSelector = string.Join(",", selectors);

                var styles = $@"
                    {combinedSelector} {{
                        border: 3px dashed red;
                    }}";

                var script = $@"
var style = document.createElement('style');
style.type = 'text/css';
style.innerHTML = `{styles}`;
document.getElementsByTagName('head')[0].appendChild(style);";

                _driver.ExecuteScript(script);
            }
        }
    }

    public sealed class DeviceScreenshotter
    {
        private readonly Project _project;
        private ScreenshotOptions Options => _project.Options.ScreenshotOptions;
        private FileProjectStore Store => (FileProjectStore)_project.Store;

        private List<Uri> PageUris => _project.Input.SelectedPages;

        private readonly string _screenshotDir;

        private readonly DateTime _creationTimestamp = DateTime.Now;

        public DeviceScreenshotter(Project project)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            _project = project;
            _screenshotDir = GetImageDir();
        }

        public async Task TakeScreenshotsAsync(CancellationToken? token = null, IProgress<TaskProgress> progress = null)
        {
            CreateEmptyDir();

            var results = new ScreenshotResults(_creationTimestamp);

            using (var ss = new Screenshotter(_project))
            {
                int i = 0;
                foreach (var uri in PageUris)
                {
                    if (token?.IsCancellationRequested == true)
                    {
                        throw new TaskCanceledException("The screenshotting task was canceled.");
                    }

                    var currentProgress = new TaskProgress(++i, PageUris.Count, uri.AbsoluteUri);
                    progress?.Report(currentProgress);
                    var result = new DeviceScreenshots(uri);
                    await ScreenshotPageAsAllDevices(ss, uri, result);
                    results.Screenshots.Add(result);
                }
            }

            string sessionLabel = Path.GetFileName(_screenshotDir);
            Store.SaveScreenshotManifest(sessionLabel, results);
            var completedProgress = new TaskProgress(PageUris.Count, PageUris.Count, "Completed");
            progress?.Report(completedProgress);
        }

        private async Task ScreenshotPageAsAllDevices(
               Screenshotter ss,
               Uri url,
               DeviceScreenshots result)
        {
            var sizes = Options.DeviceOptions
                .Where(x => x.Value.Enabled && x.Value.DisplayWidthInPixels > 0)
                .Select(x =>
                    new KeyValuePair<Device, int>(
                        x.Key,
                        x.Value.DisplayWidthInPixels));

            foreach (var size in sizes)
            {
                await ScreenshotPageAsDeviceAsync(ss, url, result, size);
            }
        }

        private void CreateEmptyDir()
        {
            // Delete existing files
            // TODO: Prompt first?
            if (Directory.Exists(_screenshotDir))
            {
                Directory.Delete(_screenshotDir, true);
            }
            Directory.CreateDirectory(_screenshotDir);
        }

        // Returns <relative, absolute> directory paths
        private string GetImageDir() =>
            Options.StoreInTimestampedDir == true
                ? Utils.CreateTimestampDirectory(Store.ScreenshotDir, _creationTimestamp)
                : Path.Combine(Store.ScreenshotDir, "default");

        private async Task ScreenshotPageAsDeviceAsync(
                Screenshotter ss,
                Uri url,
                DeviceScreenshots result,
                KeyValuePair<Device, int> size)
        {
            var device = size.Key;
            var width = size.Value;
            var baseName = Utils.SanitizeFilename(url.ToString());
            var filename = $"{baseName}.{device}{Screenshotter.ImageExtension}";
            var imgPath = Path.Combine(_screenshotDir, filename);

            NavigationTiming TakeScreenshot() =>
                ss.TakeScreenshot(url.ToString(), imgPath, width);

            try
            {
                result.RequestTiming = await Task.Run(TakeScreenshot);
                result.Paths.Add(device, filename);
            }
            catch (Exception ex)
            {
                result.Error += ex.Message + Environment.NewLine;
            }
        }
    }
}