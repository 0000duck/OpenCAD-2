using System.ComponentModel;
using OpenCAD.Kernel.Modeling;

namespace OpenCAD.Desktop.Models
{
    public interface IItemModel : INotifyPropertyChanged
    {
        string Name { get; }
        string Contents { get; set; }
        IModel Model { get; set; }
        //void Save();
        //void 
    }
}