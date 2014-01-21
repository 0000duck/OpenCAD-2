using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Caliburn.Micro;
using OpenCAD.Desktop.Misc;
using OpenCAD.Kernel.Modeling;
using OpenCAD.Kernel.Scripting;
using OpenCAD.Kernel.Structure;
using Roslyn.Compilers.CSharp;
using Roslyn.Scripting.CSharp;

namespace OpenCAD.Desktop.ViewModels
{
    public class ItemViewModel : AvalonViewModelBaseBase
    {
        private readonly Func<IModel, RendererViewModel> _rendererViewModelBuilder;
        private readonly ProjectManager _manager;
        private RendererViewModel _renderer;

        public RendererViewModel Renderer
        {
            get { return _renderer; }
            set
            {
                _renderer = value;
                NotifyOfPropertyChange(() => Renderer);
            }
        }

        public BindableCollection<MenuItemViewModel> MenuItems { get; set; }

        public ItemViewModel( Func<IModel, RendererViewModel> rendererViewModelBuilder, ProjectManager manager)
        {
            _rendererViewModelBuilder = rendererViewModelBuilder;
            _manager = manager;

            Title = "test";
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
            //
            //Renderer = _rendererViewModelBuilder(new PartScriptRunner().Execute(_manager.Project.References,Model.Contents));

            //try
            //{
            //    var scriptEngine = new ScriptEngine();
            //    scriptEngine.AddReference(Assembly.GetExecutingAssembly().Location);
            //    scriptEngine.AddReference(typeof(ScriptHost).Assembly.Location);
            //    foreach (var reference in _manager.Project.References)
            //    {
            //        scriptEngine.AddReference(reference);
            //    }
            //    var session = scriptEngine.CreateSession(new ScriptHost());

            //    using (var memoryStream = new MemoryStream())
            //    {
            //        //session.CompileSubmission<object>("var x = 10; x == 10").Compilation.Emit(memoryStream);
            //        //var compiledAssembly = memoryStream.ToArray();
            //    }
            //    Debug.WriteLine(session.Execute(Model.Contents));
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //}
        }
    }


}
