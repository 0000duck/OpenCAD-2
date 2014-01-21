using OpenCAD.Kernel.Structure;

namespace OpenCAD.Desktop.Commands
{
    public class ProjectOpenedEvent
    {
        public IProject Project { get; private set; }
        public ProjectOpenedEvent(IProject project)
        {
            Project = project;
        }
    }
}