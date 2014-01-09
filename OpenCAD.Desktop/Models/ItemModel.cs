using Caliburn.Micro;

namespace OpenCAD.Desktop.Models
{
    public class ItemModel : PropertyChangedBase, IItemModel
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string Contents { get; set; }
    }
    
}