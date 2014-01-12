using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using OpenCAD.Desktop.Misc;

namespace OpenCAD.Desktop.Models
{
    public class ProjectModel : PropertyChangedBase, IProjectModel
    {
        public string Name { get; set; }
        public string Directory { get; set; }
        public IEnumerable<string> References { get; set; } 

        private IReadOnlyObservableCollection<IItemModel> _readOnlyParts;

        IReadOnlyObservableCollection<IItemModel> IProjectModel.Items
        {
            get { return _readOnlyParts; }
        }

        private ObservableCollection<ItemModel> _items;
        public ObservableCollection<ItemModel> Items
        {
            get { return _items; }
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                _readOnlyParts = value.WrapReadOnly<ItemModel, IItemModel>();
                NotifyOfPropertyChange(() => Items);
            }
        }
    }
}
