namespace Webshot
{
    public class SpiderOptions
    {
        /// <summary>
        /// A regex pattern of page URIs to exclude.
        /// </summary>
        public string UriBlacklistPattern { get; set; }

        /// <summary>
        /// Should the spider include domains that weren't specified in the seed list.
        /// </summary>
        public bool TrackExternalLinks { get; set; } = false;

        /// <summary>
        /// True to crawl all web pages in the site, false to only match seed URIs.
        /// </summary>
        public bool FollowLinks { get; set; } = true;
    }
}