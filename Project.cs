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
        public bool IsSubproject { get; set; } = false;
        public Options Options { get; set; } = new Options();
        public ProjectInput Input { get; set; } = new ProjectInput();
        public ProjectOutput Output { get; set; } = new ProjectOutput();

        [JsonIgnore]
        public IEnumerable<Project> Subprojects => Store.GetSubprojects();

        [JsonIgnore]
        public IProjectStore Store { get; set; }

        public Project(IProjectStore store)
        {
            Store = store;
        }

        public void Save()
        {
            Store.Save(this);
        }

        public static Project Load(IProjectStore store) => store.Load();
    }

    public class DebouncedProject
    {
        public event EventHandler Saved;

        public Project Project { get; set; }

        /// <summary>
        /// Returns a project that has all pending changes saved.
        /// </summary>
        public Project FlushedProject
        {
            get
            {
                Flush();
                return Project;
            }
        }

        private readonly Debouncer _saveDebouncer;

        public DebouncedProject(Project project = null, int delay = 2000, int maxSavesBeforeDisposal = 0)
        {
            Project = project;
            _saveDebouncer = new Debouncer(PerformSave, delay, maxSavesBeforeDisposal);
        }

        public void Save(bool immediate = false)
        {
            if (immediate)
            {
                _saveDebouncer.Cancel();
                PerformSave();
                return;
            }
            _saveDebouncer.Call();
        }

        private void PerformSave()
        {
            Project?.Save();
            Saved?.Invoke(this, EventArgs.Empty);
        }

        public void Flush()
        {
            _saveDebouncer.Flush();
        }
    }

    public class ProjectOutput
    {
        /// <summary>
        /// All recursively linked web pages and broken links.
        /// </summary>
        public CrawlResults CrawlResults { get; set; } = new CrawlResults();

        public ScreenshotResults Screenshots { get; set; } = new ScreenshotResults();
    }

    public interface IObjectStore<T>
    {
        void Save(T obj);

        T Load();
    }

    public interface IProjectStore : IObjectStore<Project>
    {
        IEnumerable<Project> GetSubprojects();

        Image GetImage(ScreenshotFile file);
    }
}