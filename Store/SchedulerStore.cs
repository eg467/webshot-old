using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshot.Store
{
    /// <summary>
    /// Stores the projects that are scheduled to be screenshotted automatically.
    /// </summary>
    internal class SchedulerStore
    {
        private readonly FileStore<SchedulerSettings> _store;

        public string SchedulerFile => "ScheduledProjects.json";

        public SchedulerStore()
        {
            _store = new FileStore<SchedulerSettings>(SchedulerFile);
        }

        public SchedulerSettings Load()
        {
            if (!_store.FileExists)
            {
                Save(new SchedulerSettings());
            }
            return _store.Load();
        }

        public void Save(SchedulerSettings settings)
        {
            _store.Save(settings);
        }
    }

    internal class SchedulerSettings
    {
        public bool Enabled { get; set; }
        public List<ScheduledProject> ScheduledProjects { get; set; } = new List<ScheduledProject>();
    }

    public class ScheduledProject
    {
        /// <summary>
        /// The file path or identifier for the project.
        /// </summary>
        public string ProjectId { get; set; }

        public bool Enabled { get; set; } = true;

        public DateTime? LastRun { get; set; }
        public bool RunImmediately { get; set; }
        public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(60);
    }
}