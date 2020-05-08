using System.Collections.Generic;

namespace Webshot
{
    public class ScreenshotResult
    {
        public string Uri { get; set; }
        public Dictionary<Device, string> Paths { get; set; } = new Dictionary<Device, string>();

        public string Error { get; set; }
    }
}