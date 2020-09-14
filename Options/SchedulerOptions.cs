using System;

namespace Webshot
{
    public class SchedulerOptions
    {
        public int IntervalInMins { get; set; } = 30;
        public DateTime? LastRun { get; set; }

        public DateTime? NextScheduledRun
        {
            get
            {
                if (!LastRun.HasValue) return DateTime.Now;
                return LastRun.Value.AddMinutes(IntervalInMins);
            }
        }

        public bool IsScheduled => NextScheduledRun.Value < DateTime.Now;
    }
}