using Caliburn.Micro;

namespace OpenCAD.Desktop.ViewModels
{
    public abstract class AvalonViewModelBaseBase : PropertyChangedBase
    {
        public virtual string Title { get; protected set; }
    }
}