using System;
using Caliburn.Micro;
using OpenCAD.Desktop.Misc;
using OpenCAD.Kernel.Modeling;
using OpenCAD.Kernel.Scripting;
using OpenCAD.Kernel.Structure;

namespace OpenCAD.Desktop.ViewModels
{
    public class PartItemViewModel : AvalonViewModelBaseBase
    {
        private readonly Func<IModel, RendererViewModel> _rendererViewModelBuilder;
        private readonly ProjectManager _manager;
        public PartProjectItem Item { get; private set; }
        public BindableCollection<MenuItemViewModel> MenuItems { get; set; }
        private RendererViewModel _renderer;
        private string _source;

        public RendererViewModel Renderer
        {
            get { return _renderer; }
            set
            {
                _renderer = value;
                NotifyOfPropertyChange(() => Renderer);
            }
        }

        public string Source
        {
            get { return _source; }
            set
            {
                if (value == _source) return;
                _source = value;
                NotifyOfPropertyChange(() => Source);
            }
        }

        public PartItemViewModel(PartProjectItem item, Func<IModel, RendererViewModel> rendererViewModelBuilder, ProjectManager manager)
        {
            _rendererViewModelBuilder = rendererViewModelBuilder;
            _manager = manager;
            Item = item;
            Source = Item.Load();
            Title = "Item: " + Item.Name;
            MenuItems = new BindableCollection<MenuItemViewModel> {
                new MenuItemViewModel {
                    Header = "_Compile"
                },
                new MenuItemViewModel {
                    Header = "_Run",
                    Action = Run
                }
            };
            Renderer = rendererViewModelBuilder(new TestPart().Generate());
        }

        private void Run()
        {
            var t = new ScriptRunner().Execute(_manager.Project.References, Source);
            if (t != null)
            {
                Renderer = _rendererViewModelBuilder(t);
            }

        }
    }
}