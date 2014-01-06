using OpenCAD.Desktop.Models;

namespace OpenCAD.Desktop.Commands
{
    public class ProjectOpenedEvent
    {
        public ProjectModel Project { get; private set; }
        public ProjectOpenedEvent(ProjectModel project)
        {
            Project = project;
        }
    }
}