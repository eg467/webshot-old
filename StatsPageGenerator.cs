using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshot.Store;

namespace Webshot
{
    public class StatsPageGenerator
    {
        private readonly IProjectStoreFactory _projectStoreFactory = new FileProjectStoreFactory();

        private const string DefaultFilename = "WebPagePerformance.html";
        private string _filePath = Properties.Settings.Default.PerformanceFilePath;

        public string FilePath
        {
            get => string.IsNullOrEmpty(_filePath) ? DefaultFilename : _filePath;
            set { _filePath = value; }
        }

        private FullChartData<PageRequestChartData> LoadAggregateStats()
        {
            var _schedulerStore = new SchedulerStore();
            SchedulerSettings settings = _schedulerStore.Load();
            var allProjectStats = settings.ScheduledProjects
                .Select(s => _projectStoreFactory.Create(s.ProjectId))
                .Where(s => s.Exists)
                .Select(s => new
                {
                    Store = s,
                    Project = s.Load(),
                    Sessions = s.GetAllResults()?.Values.Cast<ScreenshotResults>()
                });

            var output = new FullChartData<PageRequestChartData>();

            foreach (var projectStats in allProjectStats)
            {
                var projectData = new ProjectChartData<PageRequestChartData>()
                {
                    ProjectName = projectStats.Project.Name,
                    ProjectId = projectStats.Store.Key,
                };
                output.Projects.Add(projectData);

                foreach (var session in projectStats.Sessions)
                {
                    foreach (DeviceScreenshots s in session.Screenshots)
                    {
                        var uri = s.Uri;
                        var pageData = projectData.PageData.FirstOrDefault(x => x.Uri.Equals(s.Uri));
                        if (pageData is null)
                        {
                            pageData = new PageChartData<PageRequestChartData>() { Uri = uri };
                            projectData.PageData.Add(pageData);
                        }
                        var stats = new PageRequestChartData(session.Timestamp, s.RequestTiming);
                        pageData.Stats.Add(stats);
                    }
                }
            }

            foreach (var project in output.Projects)
            {
                // Sort by URI
                project.PageData.Sort((a, b) => a.Uri.AbsoluteUri.CompareTo(b.Uri.AbsoluteUri));

                // Sort all session results by timestamp
                foreach (var page in project.PageData)
                {
                    page.Stats.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
                }
            }

            return output;
        }

        public StatsPageGenerator(string path = null)
        {
            if (!string.IsNullOrEmpty(path))
            {
                FilePath = path;
            }
        }

        // TODO: Make Private
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public void SavePage()
        {
            FullChartData<PageRequestChartData> stats = LoadAggregateStats();
            string serializedStats = JsonConvert.SerializeObject(stats);
            string template = File.ReadAllText("StatPageHtmlTemplate.html");
            string finalHtml = template.Replace("/***allData***/", $@"let allData=({serializedStats});");

            // Save to a dummy file first, because people viewing the html file can cause access IO errors.
            var safePath = $"{FilePath}.DONOTOPEN";
            File.WriteAllText(safePath, finalHtml, Encoding.UTF8);

            // Now write to a viewable path/extension that can fail.
            try
            {
                File.WriteAllText(FilePath, finalHtml, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Logger.Default.Log($"Could not copy performance HTML file because: {ex.Message}", LogEntryType.Error);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T">some representation of the page request data</typeparam>
    public class PageChartData<T>
    {
        public Uri Uri { get; set; }
        public List<T> Stats { get; set; } = new List<T>();
    }

    public class FullChartData<T>
    {
        public List<ProjectChartData<T>> Projects { get; set; } = new List<ProjectChartData<T>>();
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    public class GoogleFormattedChartData : FullChartData<dynamic[]>
    {
        public string[] ColumnHeaders { get; set; }
    }

    public class ProjectChartData<T>
    {
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public List<PageChartData<T>> PageData { get; set; } = new List<PageChartData<T>>();
    }

    public class PageRequestChartData
    {
        public DateTime Timestamp { get; set; }
        public NavigationTiming Timing { get; set; } = new NavigationTiming();

        public PageRequestChartData()
        {
        }

        public PageRequestChartData(DateTime timestamp, NavigationTiming timing)
        {
            Timestamp = timestamp;
            Timing = timing;
        }
    }
}