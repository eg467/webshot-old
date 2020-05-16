using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private readonly ScreenshotterOptions _options;

        public Screenshotter(ScreenshotterOptions options)
        {
            _driver = CreateDriver();
            _options = options;
        }

        public Screenshotter() : this(new ScreenshotterOptions())
        {
        }

        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
        }

        private ChromeDriver CreateDriver()
        {
            var driver = new ChromeDriver();
            return driver;
        }

        public static string SanitizeFilename(string filename)
        {
            var sanitized = System.Text.RegularExpressions.Regex.Replace(filename, "[^-a-zA-Z0-9]+", "_");
            sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, "__+", "_");
            return sanitized;
        }

        public string TakeScreenshot(string url, string filePath, int? width = null)
        {
            var outputDir = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(outputDir);
            _driver.Navigate().GoToUrl(url);

            ResizeWindow();
            HighlightBrokenLinks();
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

            void HighlightBrokenLinks()
            {
                if (_options?.BrokenLinksToHighlight?.Any() != true)
                {
                    return;
                }

                var selectors = _options.BrokenLinksToHighlight.Select(l => $"a[href='{l}']");
                var combinedSelector = string.Join(",", selectors);

                var styles = $@"
                    {combinedSelector}::after {{
                        content: 'BROKEN LINK';
                        background-color: red;
                        color: white;
                        font-weight: bold;
                        padding: 3px;
                    }}";

                var script = $@"document.write('<style type=""text/css"">{styles}</style>');";
                _driver.ExecuteScript(script);
            }
        }
    }

    public class ScreenshotterOptions
    {
        public IEnumerable<string> BrokenLinksToHighlight { get; set; } =
            Enumerable.Empty<string>();
    }
}