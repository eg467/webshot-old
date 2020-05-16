using System;
using System.IO;

namespace Webshot.Screenshots
{
    public class ScreenshotFile
    {
        public ScreenshotFile(DeviceScreenshots result, Device device)
        {
            this.Result = result ?? throw new ArgumentNullException(nameof(result));
            this.Device = device;
        }

        /// <summary>
        /// The web page screenshots of all devices.
        /// </summary>
        public DeviceScreenshots Result { get; }

        /// <summary>
        /// The device type of this screenshot
        /// </summary>
        public Device Device { get; }

        public string GetPath(string basePath) =>
            Path.Combine(basePath, Result.Paths[Device]);
    }
}