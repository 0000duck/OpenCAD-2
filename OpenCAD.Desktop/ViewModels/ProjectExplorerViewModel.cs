using Caliburn.Micro;
using OpenCAD.Desktop.Commands;

namespace OpenCAD.Desktop.ViewModels
{
    public class ProjectExplorerViewModel : AvalonViewModelBaseBase, IHandle<ProjectOpenedEvent>
    {
        public void Handle(ProjectOpenedEvent message)
        {
            throw new System.NotImplementedException();
        }
    }
}