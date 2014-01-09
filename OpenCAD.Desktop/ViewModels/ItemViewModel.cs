using System;
using System.Diagnostics;
using Caliburn.Micro;
using OpenCAD.Desktop.Models;
using OpenCAD.Kernel.Modeling;
using Roslyn.Scripting.CSharp;

namespace OpenCAD.Desktop.ViewModels
{
    public class ItemViewModel : AvalonViewModelBaseBase
    {
        private IItemModel _model;
        public IItemModel Model
        {
            get { return _model; }
            set
            {
                NotifyOfPropertyChange(() => Model);
                _model = value;
            }
        }

        public RendererViewModel Renderer { get; set; }
        public BindableCollection<MenuItemViewModel> MenuItems { get; set; }

        public ItemViewModel(IItemModel model, Func<IModel,RendererViewModel> rendererViewModelBuilder)
        {
            Model = model;
            Title = model.Name;
            Renderer = rendererViewModelBuilder(new TestPart().Generate());

            MenuItems = new BindableCollection<MenuItemViewModel> {
                new MenuItemViewModel {
                    Header = "_Compile"
                },
                new MenuItemViewModel {
                    Header = "_Run",
                    Action = Test
        
                }
            };
        }

        private void Test()
        {
            var scriptEngine = new ScriptEngine();
            scriptEngine.AddReference(typeof(Debug).Assembly.Location);

            var session = scriptEngine.CreateSession();
            try
            {
                Debug.WriteLine(session.Execute(Model.Contents));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
            
        }
    }
}
