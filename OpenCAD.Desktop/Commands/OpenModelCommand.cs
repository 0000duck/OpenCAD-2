using OpenCAD.Desktop.Models;
using OpenCAD.Kernel.Modeling;

namespace OpenCAD.Desktop.Commands
{
    public class OpenModelCommand
    {
        public IItemModel ItemModel { get; private set; }
        public OpenModelCommand(IItemModel itemModel)
        {
            ItemModel = itemModel;
        }
    }
}