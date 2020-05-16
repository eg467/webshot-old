namespace Webshot
{
    public class DeviceScreenshotOptions
    {
        public Device Device { get; set; }

        /// <summary>
        /// Pixel width of the emulated device. Image sizes may not match this.
        /// </summary>
        public int DisplayWidthInPixels { get; set; }

        /// <summary>
        /// True to take screenshots for this device
        /// </summary>
        public bool Enabled { get; set; } = true;

        public DeviceScreenshotOptions(Device device, int displayWidthInPixel, bool enabled = true)
        {
            Device = device;
            DisplayWidthInPixels = displayWidthInPixel;
            Enabled = enabled;
        }

        public DeviceScreenshotOptions()
        {
        }
    }
}