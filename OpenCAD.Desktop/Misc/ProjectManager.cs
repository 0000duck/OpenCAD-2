using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using OpenCAD.Desktop.Commands;
using OpenCAD.Desktop.Models;

namespace OpenCAD.Desktop.Misc
{
    public class ProjectManager : IHandle<OpenProjectCommand>, IHandle<AddPartCommand>
    {
        private readonly IEventAggregator _eventAggregator;

        protected ProjectModel Project { get; private set; }

        public ProjectManager(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Handle(OpenProjectCommand message)
        {
            Project = new ProjectModel();
            _eventAggregator.Publish(new ProjectOpenedEvent(Project));
        }

        public void Handle(AddPartCommand message)
        {

        }
    }
}
