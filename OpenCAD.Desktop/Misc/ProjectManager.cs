using Caliburn.Micro;
using Microsoft.Win32;
using OpenCAD.Desktop.Commands;
using OpenCAD.Kernel.Scripting;
using OpenCAD.Kernel.Structure;

namespace OpenCAD.Desktop.Misc
{
    public class ProjectManager : IHandle<OpenProjectCommand>, IHandle<AddPartCommand>, IHandle<OpenProjectDialog>
    {
        private readonly IEventAggregator _eventAggregator;


        public IProject Project { get; private set; }

        public ProjectManager(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Handle(OpenProjectCommand message)
        {
            Project = new JsonProject(message.FileName);
            _eventAggregator.Publish(new ProjectOpenedEvent(Project));
        }

        public void Handle(AddPartCommand message)
        {

        }

        public void Handle(OpenProjectDialog message)
        {
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".cadproj",
                Filter = "OpenCAD Project|*.cadproj"
            };
            if (dlg.ShowDialog() == true)
            {
                _eventAggregator.Publish(new OpenProjectCommand(dlg.FileName));

            }
        }
    }
}
