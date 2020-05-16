using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using Webshot.Controls;
using Webshot.Forms;
using Webshot.Screenshots;

namespace Webshot
{
    public class FileProjectStore : IProjectStore
    {
        public const string SubProjectDirName = "SubProjects";
        public const string ProjectFilename = "webshots.wsproj";

        private FileStore<Project> _filestore;

        public bool IsSaved => DirectoryContainsProject(ProjectDir);

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
        }

        public static Project CreateOrLoadProject(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            var store = new FileProjectStore(directory);

            if (DirectoryContainsProject(directory))
            {
                return store.Load();
            }
            else
            {
                Directory.CreateDirectory(directory);
                var project = new Project(store);
                project.Save();
                return project;
            }
        }

        public string ProjectPath => _filestore.FilePath;

        public static string UserAppProjects =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                nameof(Webshot),
                "Projects");

        /// <summary>
        ///
        /// </summary>
        /// <param name="temporary">
        /// True if the data should be stored in a temp directory,
        /// False to store in the application directory</param>
        /// <returns></returns>
        public static string CreateProjectDirectory(bool temporary)
        {
            string TempDir() => Path.Combine(Path.GetTempPath(), nameof(Webshot));
            string basePath = temporary ? TempDir() : UserAppProjects;
            var timestamp = DateTime.Now.Timestamp();

            var path = Path.Combine(basePath, timestamp);
            if (Directory.Exists(path))
            {
                path = Path.Combine(path, $"-{Guid.NewGuid()}");
            }
            Directory.CreateDirectory(path);
            return path;
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
            SaveToRecentProjectsList();
        }

        private void SaveToRecentProjectsList()
        {
            Utils.ChangeSettings(s =>
            {
                var recentProjects =
                    s.RecentProjects ?? new StringCollection();

                recentProjects.Remove(this.ProjectDir);
                recentProjects.Add(this.ProjectDir);
                s.RecentProjects = recentProjects;
            });
        }

        public static bool DirectoryContainsProject(string directory) =>
            directory != null && File.Exists(Path.Combine(directory, ProjectFilename));

        public IEnumerable<Project> GetSubprojects()
        {
            var fullSubprojectDir = Path.Combine(ProjectDir, SubProjectDirName);
            return Directory.Exists(fullSubprojectDir)
                ? Directory.EnumerateDirectories(fullSubprojectDir)
                    .Where(DirectoryContainsProject)
                    .Select(d => new FileProjectStore(d))
                    .Select(s => new Project(s))
                : Enumerable.Empty<Project>();
        }

        public Image GetImage(ScreenshotFile file)
        {
            var relPath = file.Result.Paths[file.Device].Trim('\\');
            var path = Path.Combine(ProjectDir, relPath);
            return Image.FromFile(path);
        }
    }

    public interface IOptionsSaver
    {
        void SaveOptions(Options options);
    }

    public interface IOutputSaver
    {
        void SaveOutput(ProjectOutput output);
    }
}