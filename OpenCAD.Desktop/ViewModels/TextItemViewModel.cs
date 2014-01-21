using Caliburn.Micro;
using OpenCAD.Kernel.Structure;

namespace OpenCAD.Desktop.ViewModels
{
    public class TextItemViewModel : AvalonViewModelBaseBase
    {
        public IProjectItem Item { get; private set; }
        public BindableCollection<MenuItemViewModel> MenuItems { get; set; }
        public TextItemViewModel(IProjectItem item)
        {
            Item = item;
            Title = "Item: " + Item.Name;
            MenuItems = new BindableCollection<MenuItemViewModel> {
                new MenuItemViewModel {
                    Header = "_hello"
                }
            };
        }
    }
}