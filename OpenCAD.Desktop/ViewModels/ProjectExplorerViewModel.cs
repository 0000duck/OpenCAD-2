using System;
using System.Diagnostics;
using Caliburn.Micro;
using OpenCAD.Desktop.Commands;
using OpenCAD.Kernel.Structure;

namespace OpenCAD.Desktop.ViewModels
{
    public class ProjectExplorerViewModel : AvalonViewModelBaseBase, IHandle<ProjectOpenedEvent>
    {
        private readonly IEventAggregator _eventAggregator;
        private IProject _project;

        public bool ProjectIsVisible
        {
            get { return Project != null; }
        }
        public bool ButtonsIsVisible
        {
            get { return Project == null; }
        }

        public IProject Project
        {
            get { return _project; }
            set
            {
                if (Equals(value, _project)) return;
                _project = value;
                NotifyOfPropertyChange(() => Project);
                NotifyOfPropertyChange(() => ProjectIsVisible);
                NotifyOfPropertyChange(() => ButtonsIsVisible);
            }
        }

        public ProjectExplorerViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Title = "Project Explorer";
        }

        public void Handle(ProjectOpenedEvent message)
        {
            Project = message.Project;
        }

        public void LoadProject()
        {
            _eventAggregator.Publish(new OpenProjectDialog());
        }

        public void Open(IProjectItem item)
        {
           _eventAggregator.Publish(new OpenItemCommand(item));
        }
    }
}