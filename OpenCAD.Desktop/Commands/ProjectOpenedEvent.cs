using OpenCAD.Desktop.Models;

namespace OpenCAD.Desktop.Commands
{
    public class ProjectOpenedEvent
    {
        public IProjectModel Project { get; private set; }
        public ProjectOpenedEvent(IProjectModel project)
        {
            Project = project;
        }
    }
}