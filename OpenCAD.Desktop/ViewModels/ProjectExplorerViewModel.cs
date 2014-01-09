using System;
using System.Diagnostics;
using Caliburn.Micro;
using OpenCAD.Desktop.Commands;
using OpenCAD.Desktop.Models;

namespace OpenCAD.Desktop.ViewModels
{
    public class ProjectExplorerViewModel : AvalonViewModelBaseBase, IHandle<ProjectOpenedEvent>
    {
        private readonly IEventAggregator _eventAggregator;
        private IProjectModel _project;
        public IProjectModel Project
        {
            get { return _project; }
            set
            {
                if (Equals(value, _project)) return;
                _project = value;
                NotifyOfPropertyChange(() => Project);
                //NotifyOfPropertyChange(() => ProjectIsVisible);
                ///NotifyOfPropertyChange(() => ButtonsIsVisible);
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

        public void ShowModel(IItemModel itemModel)
        {
            _eventAggregator.Publish(new OpenModelCommand(itemModel));
        }
    }
}