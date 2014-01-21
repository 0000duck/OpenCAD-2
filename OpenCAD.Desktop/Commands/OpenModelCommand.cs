using OpenCAD.Kernel.Modeling;
using OpenCAD.Kernel.Structure;

namespace OpenCAD.Desktop.Commands
{
    public class OpenItemCommand
    {
        public IProjectItem Item { get; private set; }
        public OpenItemCommand(IProjectItem item)
        {
            Item = item;
        }
    }
}