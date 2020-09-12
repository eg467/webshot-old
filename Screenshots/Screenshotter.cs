using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

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
            _options = options;
            _brokenLinks = brokenLinks.ToDictionary(x => x.Target, x => x);
            _projectCredentials = projectCredentials;
            _driver = CreateDriver();
        }

        public Screenshotter() : this(
            new ScreenshotOptions(),
            new List<BrokenLink>(),
            new ProjectCredentials())
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

        public static string SanitizeFilename(string filename)
        {
            var sanitized = System.Text.RegularExpressions.Regex.Replace(filename, "[^-a-zA-Z0-9]+", "_");
            sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, "__+", "_");
            return sanitized;
        }

        /// <summary>
        /// Takes a screenshot of a web page.
        /// </summary>
        /// <param name="url">The URL of the web page to screenshoot.</param>
        /// <param name="filePath">The image file to save to.</param>
        /// <param name="width">The device width of the browser</param>
        /// <returns>The full file path of the saved image</returns>
        public string TakeScreenshot(string url, string filePath, int? width = null)
        {
            var outputDir = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(outputDir);
            _driver.Navigate().GoToUrl(url);

            ResizeWindow();
            HighlightBrokenLinks(url);

            int screenshotDelay = 1000;
            Thread.Sleep(screenshotDelay);

            var screenshot = _driver.GetScreenshot();
            screenshot.SaveAsFile(filePath, _imageFormat);
            ClearResize();
            return filePath;

            // LOCAL FUNCTIONS

            /// <summary>
            /// Repeatedly resize the window to account for lazily loaded elements.
            /// </summary>
            /// <param name="width"></param>
            void ResizeWindow()
            {
                // Adapted from https://stackoverflow.com/a/56535317
                int numTries = 0;
                const int maxTries = 5;
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
                    Dictionary<string, Object> metrics = new Dictionary<string, Object>
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
                _driver.ExecuteChromeCommand("Emulation.clearDeviceMetricsOverride", new Dictionary<string, Object>());
            }

            void HighlightBrokenLinks(string rawCallingUri)
            {
                var standardizedUri = new Uri(rawCallingUri);
                standardizedUri = standardizedUri.TryStandardize();

                if (_brokenLinks?.Any() != true)
                {
                    return;
                }

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
}