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
        public const string ScreenshotDir = "Screenshots";
        public const string ScreenshotManifestFilename = "screenshots.manifest";

        private FileStore<Project> _filestore;

        public bool ProjectExists => DirectoryContainsProject(ProjectDir);

        public string ProjectDir
        {
            get => Path.GetDirectoryName(_filestore.FilePath);
            set
            {
                var filePath = Path.Combine(value, ProjectFilename);
                _filestore = new FileStore<Project>(filePath);
            }
        }

        public FileProjectStore()
        {
        }

        public FileProjectStore(string projectDir) : this()
        {
            ProjectDir = projectDir;
            Directory.CreateDirectory(projectDir);
        }

        public static string UserAppProjects =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                nameof(Webshot),
                "Projects");

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

        public Project Create()
        {
            var project = new Project(this);
            project.Save();
            SaveToRecentProjectsList();
            return project;
        }

        public Project Load()
        {
            var project = _filestore.Load();
            project.Store = this;
            SaveToRecentProjectsList();
            return project;
        }

        public void Save(Project project)
        {
            _filestore.Save(project);
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

        public Image GetImage(ScreenshotFile file)
        {
            var relPath = file.Result.Paths[file.Device].Trim('\\');
            var path = Path.Combine(ProjectDir, relPath);
            return Image.FromFile(path);
        }

        public void SaveScreenshotManifest(string label, ScreenshotResults results)
        {
            var screenshotDir = Path.Combine(
                ProjectDir,
                ScreenshotDir,
                Utils.SanitizeFilename(label));

            Directory.CreateDirectory(screenshotDir);

            var manifestPath = Path.Combine(screenshotDir, ScreenshotManifestFilename);

            var store = new FileStore<ScreenshotResults>(manifestPath);
            store.Save(results);
        }

        public Dictionary<string, ScreenshotResults> GetScreenshots()
        {
            var screenshotPath = Path.Combine(ProjectDir, ScreenshotDir);

            string GetManifestPath(string dir) =>
                Path.Combine(dir, ScreenshotManifestFilename);

            string GetDirectoryName(string manifestPath) =>
                Path.GetFileName(Path.GetDirectoryName(manifestPath));

            ScreenshotResults ReadManifest(string path)
            {
                var store = new FileStore<ScreenshotResults>(path);
                return store.Load();
            }

            // Each subdirectory under the output directory represents a session of screenshots.
            return Directory.GetDirectories(screenshotPath)
                .Select(GetManifestPath)
                .Where(File.Exists)
                .ToDictionary(GetDirectoryName, ReadManifest);
        }
    }
}