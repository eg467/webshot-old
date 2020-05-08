using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Webshot
{
    public sealed class Screenshotter : IDisposable
    {
        private readonly ChromeDriver _driver;
        private const int ScreenshotDelayMs = 250;

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
        public async Task<string> TakeScreenshot(string url, string filePath, int? width = null)
        {
            if (!filePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                filePath += ".png";
            }
            string outputDir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            _driver.Navigate().GoToUrl(url);

            return await TakeDriverScreenshot(filePath, width);
        }

        public async Task<string> TakeScreenshot(string url, int? width = null)
        {
            var filePath = GetFilePathFromUri(url);
            return await TakeScreenshot(url, filePath, width);
        }

        /// <summary>
        /// Saves a screenshot from a URL. Adapted from https://stackoverflow.com/a/56535317.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private async Task<string> TakeDriverScreenshot(string filePath, int? width = null)
        {
            ResizeWindow(width);
            _driver.GetScreenshot().SaveAsFile(filePath, ScreenshotImageFormat.Png);
            ClearResize();

            // Dummy async for now
            await Task.CompletedTask;

            return filePath;
        }

        /// <summary>
        /// Repeatedly resize the window to account for lazily loaded elements.
        /// </summary>
        /// <param name="width"></param>
        private void ResizeWindow(int? width)
        {
            int numTries = 0;
            const int maxTries = 5;
            int prevHeight;
            int calculatedHeight = -1;
            int CalculateHeight()
            {
                // Sometimes a long, sometimes double, etc
                object result = _driver.ExecuteScript("return Math.max(document.body.scrollHeight,document.body.offsetHeight,document.documentElement.clientHeight, document.documentElement.scrollHeight,document.documentElement.offsetHeight,document.documentElement.getBoundingClientRect().height)");
                return Convert.ToInt32(result) + 1;
            }

            do
            {
                prevHeight = calculatedHeight;
                const string autoWidth = "return Math.max(window.innerWidth,document.body.scrollWidth,document.documentElement.scrollWidth)";
                var calculatedWidth = width.HasValue ? $"return {width}" : autoWidth;
                calculatedHeight = CalculateHeight();
                Debug.WriteLine($"Height for width ({width}) at {calculatedHeight}");
                Dictionary<string, Object> metrics = new Dictionary<string, Object>
                {
                    ["width"] = _driver.ExecuteScript(calculatedWidth),
                    ["height"] = calculatedHeight,
                    //["height"] = _driver.ExecuteScript("return 5000"),
                    ["deviceScaleFactor"] = (double)_driver.ExecuteScript("return window.devicePixelRatio"),
                    ["mobile"] = _driver.ExecuteScript("return typeof window.orientation !== 'undefined'")
                };
                _driver.ExecuteChromeCommand("Emulation.setDeviceMetricsOverride", metrics);
            } while (calculatedHeight != prevHeight && ++numTries < maxTries);
        }

        private void ClearResize()
        {
            _driver.ExecuteChromeCommand("Emulation.clearDeviceMetricsOverride", new Dictionary<string, Object>());
        }

        private static string GetFilePathFromUri(string websiteUri)
        {
            var uri = new Uri(websiteUri);
            return SanitizeFilename($"{uri.Host}{uri.PathAndQuery}");
        }
    }
}