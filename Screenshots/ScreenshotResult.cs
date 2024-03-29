﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Webshot
{
    public class DeviceScreenshots
    {
        public Uri Uri { get; set; }

        public NavigationTiming RequestTiming { get; set; } = new NavigationTiming();

        public Dictionary<Device, string> Paths
        { get; set; } = new Dictionary<Device, string>();

        public string Error { get; set; }

        public DeviceScreenshots(Uri uri)
        {
            this.Uri = uri;
        }

        public DeviceScreenshots()
        {
        }
    }

    /// <summary>
    /// See: https://developer.mozilla.org/en-US/docs/Web/API/PerformanceNavigationTiming
    /// </summary>
    public class NavigationTiming
    {
        public int DecodedBodySize { get; set; }
        public int FetchStart { get; set; }
        public int LoadEventEnd { get; set; }
        public int LoadEventStart { get; set; }
        public int RedirectEnd { get; set; }
        public int RedirectStart { get; set; }
        public int RequestStart { get; set; }
        public int ResponseEnd { get; set; }
        public int ResponseStart { get; set; }
        public int SecureConnectionStart { get; set; }
        public int StartTime { get; set; }
        public int ConnectEnd { get; set; }
        public int ConnectStart { get; set; }
        public int DomComplete { get; set; }
        public int DomContentLoadedEventEnd { get; set; }
        public int DomContentLoadedEventStart { get; set; }
        public int DomInteractive { get; set; }
        public int DomainLookupEnd { get; set; }
        public int DomainLookupStart { get; set; }
        public int Duration { get; set; }
        public int TransferSize { get; set; }

        private TimeSpan Ts(int ms) => TimeSpan.FromMilliseconds(ms);

        public int FromFetch(int time) => time - FetchStart;

        public TimeSpan Ttfb => Ts(FromFetch(ResponseStart));

        // See https://www.w3.org/TR/2018/WD-navigation-timing-2-20181130/timestamp-diagram.svg

        [JsonIgnore]
        public TimeSpan Redirect => Ts(RedirectEnd - RedirectStart);

        [JsonIgnore]
        public TimeSpan AppCache => Ts(DomainLookupStart - FetchStart);

        [JsonIgnore]
        public TimeSpan Dns => Ts(DomainLookupEnd - DomainLookupStart);

        [JsonIgnore]
        public TimeSpan Tcp => Ts(ConnectEnd - ConnectStart);

        [JsonIgnore]
        public TimeSpan Request => Ts(ResponseStart - RequestStart);

        [JsonIgnore]
        public TimeSpan Response => Ts(ResponseEnd - ResponseStart);

        [JsonIgnore]
        public TimeSpan Processing => Ts(DomComplete - DomInteractive);

        [JsonIgnore]
        public TimeSpan Load => Ts(LoadEventEnd - LoadEventStart);

        [JsonIgnore]
        public TimeSpan BackendResource => Ts(ResponseEnd - RedirectStart);

        [JsonIgnore]
        public TimeSpan FrontendProcessing => Ts(LoadEventEnd - DomInteractive);
    }

    public class ScreenshotResults
    {
        public List<DeviceScreenshots> Screenshots { get; set; } = new List<DeviceScreenshots>();
        public DateTime Timestamp { get; set; }

        public ScreenshotResults(DateTime timestamp)
        {
            Timestamp = timestamp;
        }

        public ScreenshotResults() : this(DateTime.Now)
        {
        }

        public override string ToString() =>
            $"Screenshots from {Timestamp.ToLongTimeString()}";
    }
}