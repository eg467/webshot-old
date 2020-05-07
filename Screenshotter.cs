using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;

namespace Webshot
{
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