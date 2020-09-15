using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using Webshot.Forms;
using Webshot.Screenshots;

namespace Webshot
{
    public class Project
    {
        public string Name { get; set; } = "Webshot Project";
        public DateTime Created { get; set; } = DateTime.Now;
        public Options Options { get; set; } = new Options();
        public ProjectInput Input { get; set; } = new ProjectInput();
        public CrawlResults CrawledPages { get; set; } = new CrawlResults();
        //public ProjectOutput Output { get; set; } = new ProjectOutput();

        [JsonIgnore]
        public IProjectStore Store { get; set; }

        public Project(IProjectStore store)
        {
            Store = store;
        }

        public void Save() => Store.SaveProject(this);
    }

    /// <summary>
    /// A project that will only save once during a delay period.
    /// </summary>
    public class DebouncedProject
    {
        public event EventHandler Saved;

        public Project Project { get; set; }

        private readonly Debouncer _saveDebouncer;

        public DebouncedProject(Project project = null, int delay = 2000, int maxSavesBeforeDisposal = 0)
        {
            Project = project;
            _saveDebouncer = new Debouncer(SaveNow, delay, maxSavesBeforeDisposal);
        }

        /// <summary>
        /// Persists the project using the debouncer.
        /// </summary>
        public void Save()
        {
            _saveDebouncer.Call();
        }

        /// <summary>
        /// Saves the project immediately.
        /// </summary>
        public void SaveNow()
        {
            _saveDebouncer.Cancel();
            Project?.Save();
            Saved?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Saves the project if a save is pending.
        /// </summary>
        public void Flush()
        {
            _saveDebouncer.Flush();
        }
    }

    //public class ProjectOutput
    //{
    //    public ScreenshotResults Screenshots { get; set; } = new ScreenshotResults();
    //}

    public interface IObjectStore<T>
    {
        void SaveProject(T obj);

        T LoadProject();
    }

    public interface IProjectStore : IObjectStore<Project>
    {
        Image GetImage(string sessionId, ScreenshotFile file);

        Dictionary<string, ScreenshotResults> GetScreenshots();
    }
}