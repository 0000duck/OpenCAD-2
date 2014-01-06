using System;
using Caliburn.Micro;
using OpenCAD.Desktop.Commands;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Modeling.Octree;
using Microsoft.Win32;

namespace OpenCAD.Desktop.ViewModels
{
    public class MenuViewModel : PropertyChangedBase
    {
        private readonly IEventAggregator _eventAggregator;

        public BindableCollection<MenuItemViewModel> Items { get; set; }

        public MenuViewModel(IEventAggregator eventAggregator, Func<TeapotViewModel> teapotBuilder)//, Func<ProjectExplorerViewModel> projectExplorerViewModelBuilder, Func<EventAggregatorDebugViewModel> eventsDebugBuilder,)
        {
            _eventAggregator = eventAggregator;
            Items = new BindableCollection<MenuItemViewModel> {
                new MenuItemViewModel {
                    Header = "_FILE",
                    Items = new BindableCollection<MenuItemViewModel> {
                        new MenuItemViewModel {
                            Header = "_Open",
                            Items = new BindableCollection<MenuItemViewModel> {
                                new MenuItemViewModel {
                                    Header = "_Project",
                                    Action = () =>
                                        {
                                            var dlg = new OpenFileDialog
                                                {
                                                    DefaultExt = ".cadproj",
                                                    Filter = "OpenCAD Project|*.cadproj"
                                                };
                                            if (dlg.ShowDialog() == true)
                                            {
                                                _eventAggregator.Publish(new OpenProjectCommand(dlg.FileName));
                                            }
                                        }
                                },
                                new MenuItemViewModel {
                                    Header = "Model",
                                    Action = () => _eventAggregator.Publish(new OpenModelCommand(new TestPart().Generate()))
                                },
                                new MenuItemViewModel {
                                    Header = "Teapot",
                                    Action = () => _eventAggregator.Publish(new AddTabViewCommand {Model = teapotBuilder()})
                                },

                            }
                        },
                        new MenuItemViewModel {
                            Header = "Close",
                            Action = () => _eventAggregator.Publish(new DebugCommand("close"))
                        }
                    }
                },
                new MenuItemViewModel {
                    Header = "_EDIT"
                },
                new MenuItemViewModel {
                    Header = "_VIEW"
                },
                new MenuItemViewModel {
                    Header = "_WINDOW"
                },
                new MenuItemViewModel {
                    Header = "_HELP"
                }
            };
        }
    }
}