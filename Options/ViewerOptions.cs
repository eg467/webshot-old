using System.Collections.Generic;

namespace Webshot
{
    public class ViewerOptions
    {
        /// <summary>
        /// The scale of the displayed image for each device type
        /// </summary>
        public Dictionary<Device, int> DeviceOutputScales { get; set; } = new Dictionary<Device, int>
        {
            [Device.Desktop] = 100,
            [Device.Tablet] = 100,
            [Device.Mobile] = 100,
        };

        public int GetScale(Device device)
        {
            if (!DeviceOutputScales.TryGetValue(device, out int scale))
            {
                scale = 100;
            }
            return scale;
        }

        /// <summary>
        /// True to constrain the width of the image to its container, regardless of <seealso cref="ImageScale" />.
        /// </summary>
        public bool ConstrainImageWidth { get; set; }
    }
}