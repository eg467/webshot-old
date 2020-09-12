using System;
using System.Collections.Generic;

namespace Webshot
{
    public class DeviceScreenshots
    {
        public Uri Uri { get; set; }
        public Dictionary<Device, string> Paths { get; set; } = new Dictionary<Device, string>();

        public string Error { get; set; }

        public DeviceScreenshots(Uri uri)
        {
            this.Uri = uri;
        }

        public DeviceScreenshots()
        {
        }
    }

    public class ScreenshotResults
    {
        public List<DeviceScreenshots> Screenshots { get; set; } = new List<DeviceScreenshots>();
        public DateTime Timestamp { get; set; }

        public ScreenshotResults()
        {
            Timestamp = DateTime.Now;
        }

        public override string ToString() =>
            $"Screenshots from {Timestamp.ToLongTimeString()}";
    }
}