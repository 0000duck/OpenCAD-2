using System.ComponentModel;

namespace OpenCAD.Desktop.Models
{
    public interface IItemModel : INotifyPropertyChanged
    {
        string Name { get; }
        string Contents { get; set; }
        //void Save();
        //void 
    }
}