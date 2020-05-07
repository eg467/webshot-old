using System.Collections.Generic;

namespace Webshot
{
    public class Settings
    {
        public string RootUri { get; set; }
        public string OutputDir { get; set; }

        public bool CrawlPages { get; set; }
        public bool CrawlExternalSites { get; set; }

        /// <summary>
        /// A dictionary from the device width size to the device subdirectory name
        /// </summary>
        public Dictionary<Device, int> Devices = new Dictionary<Device, int>();

        public string[] Uris { get; set; }
    }
}