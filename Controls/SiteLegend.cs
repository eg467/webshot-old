using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Webshot.Controls
{
    public class SiteLegend
    {
        /// <summary>
        /// Key=host name, Value=footnote identifer
        /// </summary>
        private readonly Dictionary<string, string> _hosts;

        public string ExpandUri(string uri) => CondenseUri(new Uri(uri));

        private string GetSiteKey(Uri uri) =>
            $"{uri.Scheme}://{uri.Authority.ToUpperInvariant()}";

        public string CondenseUri(Uri uri) =>
            $"[{_hosts[GetSiteKey(uri)]}]{uri.PathAndQuery}{uri.Fragment}";

        public string ExpandedUri(string condensedUri)
        {
            var match = Regex.Match(condensedUri, @"^\[(?<id>[^\]]+)\](?<path>.+)$");
            if (!match.Success)
            {
                throw new ArgumentException("Invalid URI format provided.");
            }

            var site = _hosts
                .Where(h => h.Value == match.Groups["id"].Value)
                .Select(h => h.Key);
            var path = match.Groups["path"].Value;
            return site + path;
        }

        public SiteLegend(IEnumerable<Uri> uris)
        {
            _hosts = uris
               .Select(GetSiteKey)
               .Distinct()
               .Select((h, i) => new { Site = h, Index = i })
               .ToDictionary(x => x.Site, x => $"{x.Index}");
        }

        public override string ToString()
        {
            var mappings = _hosts
                .Select(x => $"[{x.Value}] = {x.Key}")
                .ToArray();

            return string.Join(Environment.NewLine, mappings);
        }
    }
}