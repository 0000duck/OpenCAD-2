using System.ComponentModel;
using OpenCAD.Desktop.Misc;

namespace OpenCAD.Desktop.Models
{
    public interface IProjectModel : INotifyPropertyChanged
    {
        string Name { get; }
        string Directory { get; }
        IReadOnlyObservableCollection<IItemModel> Items { get; }
    }
}