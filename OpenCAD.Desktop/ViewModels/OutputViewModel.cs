using Caliburn.Micro;
using OpenCAD.Desktop.Commands;

namespace OpenCAD.Desktop.ViewModels
{
    public class OutputViewModel : AvalonViewModelBaseBase, IHandle<OutputCommand>
    {
        public BindableCollection<object> OutputItems { get; set; }
        public OutputViewModel()
        {
            OutputItems = new BindableCollection<object> {};

        }

        public void Handle(OutputCommand message)
        {
            OutputItems.Add(message);
        }
    }
}