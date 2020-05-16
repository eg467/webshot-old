using System;
using System.Collections.Generic;

namespace Webshot
{
    public class SpiderOptions
    {
        public string UriBlacklistPattern { get; set; }
        public bool FollowExternalLinks { get; set; } = false;
    }
}