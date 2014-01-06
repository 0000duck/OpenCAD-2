using System.Diagnostics;
using OpenCAD.Kernel.Graphics;
using OpenCAD.Kernel.Modeling;

namespace OpenCAD.Desktop.ViewModels
{
    public class RendererViewModel : AvalonViewModelBaseBase
    {
        public IModel Model { get; private set; }

        public RendererViewModel(IModel model)
        {
            Model = model;
            Title = "Model: " + Model.Name;
        }
    }
}