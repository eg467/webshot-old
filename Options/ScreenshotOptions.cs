using System.Collections.Generic;

namespace Webshot
{
    public class ScreenshotOptions
    {
        /// <summary>
        /// True to store each iteratation of screenshots in a new directory.
        /// False to overwrite images on each iteration of screenshots.
        /// </summary>
        public bool StoreInTimestampedDir { get; set; } = false;

        public bool HighlightBrokenLinks { get; set; } = false;

        public Dictionary<Device, DeviceScreenshotOptions> DeviceOptions { get; set; } =
      new Dictionary<Device, DeviceScreenshotOptions>
      {
          [Device.Desktop] = new DeviceScreenshotOptions(Device.Desktop, 1920),
          [Device.Mobile] = new DeviceScreenshotOptions(Device.Mobile, 480),
          [Device.Tablet] = new DeviceScreenshotOptions(Device.Tablet, 768),
      };
    }
}