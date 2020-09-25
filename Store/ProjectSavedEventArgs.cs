using System;

namespace Webshot
{
    public class ProjectSavedEventArgs : EventArgs
    {
        public Project Project { get; }

        public ProjectSavedEventArgs(Project project)
        {
            Project = project;
        }
    }
}