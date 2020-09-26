using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using Webshot.Screenshots;

namespace Webshot
{
    public class FileProjectStore : IProjectStore
    {
        public const string ProjectFilename = "webshots.wsproj";
        public const string ScreenshotManifestFilename = "screenshots.manifest";

        private FileStore<Project> _filestore;

        public event EventHandler<ProjectSavedEventArgs> Saved;

        public bool ProjectExists => DirectoryContainsProject(ProjectDir);

        public string ProjectPath => _filestore.FilePath;

        public string ProjectDir
        {
            get => Path.GetDirectoryName(ProjectPath);
            set
            {
                var filePath = Path.Combine(value, ProjectFilename);
                _filestore = new FileStore<Project>(filePath);
            }
        }

        public string ScreenshotDir => Path.Combine(ProjectDir, "Screenshots");

        public FileProjectStore(string projectDir)
        {
            if (string.IsNullOrEmpty(projectDir))
            {
                throw new ArgumentNullException(nameof(projectDir));
            }
            if (projectDir.Contains(ProjectFilename))
            {
                projectDir = Path.GetDirectoryName(projectDir);
            }

            ProjectDir = projectDir;
            Directory.CreateDirectory(projectDir);
        }

        public static string UserAppProjects =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                nameof(Webshot),
                "Projects");

        public bool Exists => _filestore.FileExists;

        public string Key => ProjectPath;

        /// <summary>
        ///
        /// </summary>
        /// <param name="temporaryDir">
        /// True if the data should be stored in a temp directory,
        /// False to store in the application directory</param>
        /// <returns></returns>
        public static string CreateTempProjectDirectory(bool temporaryDir)
        {
            string GetTempDir() => Path.Combine(Path.GetTempPath(), nameof(Webshot));
            string basePath = temporaryDir ? GetTempDir() : UserAppProjects;
            var timestamp = DateTime.Now.Timestamp();

            var path = Path.Combine(basePath, timestamp);
            if (Directory.Exists(path))
            {
                path = Path.Combine(path, $"-{Guid.NewGuid()}");
            }
            Directory.CreateDirectory(path);
            return path;
        }

        public Project CreateProject()
        {
            var project = new Project(this);
            Save(project);
            SaveToRecentProjectsList();
            return project;
        }

        /// <summary>
        /// Loads a project or creates a new one if it doesn't exist.
        /// </summary>
        /// <returns></returns>
        public Project Load()
        {
            if (!ProjectExists)
            {
                return CreateProject();
            }

            var project = _filestore.Load();
            project.Store = this;
            SaveToRecentProjectsList();
            return project;
        }

        public void Save(Project project)
        {
            _filestore.Save(project);
            OnSaved(project);
        }

        private void SaveToRecentProjectsList()
        {
            Utils.ChangeSettings(s =>
            {
                var recentProjects = s.RecentProjects ?? new StringCollection();
                recentProjects.Remove(this.ProjectDir);
                recentProjects.Insert(0, this.ProjectDir);
                s.RecentProjects = recentProjects;
            });
        }

        public static bool DirectoryContainsProject(string directory) =>
            directory != null && File.Exists(Path.Combine(directory, ProjectFilename));

        public Image GetImage(string sessionId, ScreenshotFile file)
        {
            var filename = file.Result.Paths[file.Device];
            var path = Path.Combine(ScreenshotDir, sessionId, filename);
            return Image.FromFile(path);
        }

        public void SaveScreenshotManifest(string label, ScreenshotResults results)
        {
            string sanitizedLabel = Utils.SanitizeFilename(label);
            var screenshotDir = Path.Combine(this.ScreenshotDir, sanitizedLabel);

            Directory.CreateDirectory(screenshotDir);

            var manifestPath = Path.Combine(screenshotDir, ScreenshotManifestFilename);

            var store = new FileStore<ScreenshotResults>(manifestPath);
            store.Save(results);
        }

        private string GetManifestPath(string dir) =>
            Path.Combine(dir, ScreenshotManifestFilename);

        private ScreenshotResults SessionResultsFromDir(string sessionDir)
        {
            string path = GetManifestPath(sessionDir);
            if (!File.Exists(path)) return null;
            return LoadResultManifest(path);
        }

        private ScreenshotResults LoadResultManifest(string path)
        {
            var store = new FileStore<ScreenshotResults>(path);
            return store.Load();
        }

        public ScreenshotResults GetSessionResults(string sessionId)
        {
            Directory.CreateDirectory(ScreenshotDir);
            string path = Path.Combine(ScreenshotDir, sessionId);
            return SessionResultsFromDir(path);
        }

        public Dictionary<string, ScreenshotResults> GetAllResults()
        {
            string GetSessionId(string manifestPath) =>
                Path.GetFileName(Path.GetDirectoryName(manifestPath));

            Directory.CreateDirectory(ScreenshotDir);

            // Each subdirectory under the output directory represents a session of screenshots.
            return Directory.GetDirectories(ScreenshotDir)
                .Select(GetManifestPath)
                .Where(File.Exists)
                .ToDictionary(GetSessionId, LoadResultManifest);
        }

        protected void OnSaved(Project project)
        {
            var args = new ProjectSavedEventArgs(project);
            Saved?.Invoke(this, args);
        }
    }

    internal class FileProjectStoreFactory : IProjectStoreFactory
    {
        public IProjectStore Create(string projectId)
        {
            return new FileProjectStore(projectId);
        }
    }
}