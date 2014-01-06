using OpenCAD.Kernel.Modeling;

namespace OpenCAD.Desktop.Commands
{
    public class OpenModelCommand
    {
        public IModel Model { get; private set; }
        public OpenModelCommand(IModel model)
        {
            Model = model;
        }
    }
}