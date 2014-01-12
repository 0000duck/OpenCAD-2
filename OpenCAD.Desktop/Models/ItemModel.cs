using Caliburn.Micro;
using OpenCAD.Kernel.Modeling;

namespace OpenCAD.Desktop.Models
{
    public class ItemModel : PropertyChangedBase, IItemModel
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string Contents { get; set; }
        public IModel Model { get; set; }
    }
    
}