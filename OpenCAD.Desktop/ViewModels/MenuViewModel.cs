using System;
using Caliburn.Micro;
using OpenCAD.Desktop.Commands;
using OpenCAD.Desktop.Misc;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Modeling.Octree;
using Microsoft.Win32;
using OpenCAD.Kernel.Scripting;

namespace OpenCAD.Desktop.ViewModels
{
    public class MenuViewModel : PropertyChangedBase
    {
        private readonly IEventAggregator _eventAggregator;

        public BindableCollection<MenuItemViewModel> Items { get; set; }

        public MenuViewModel(IEventAggregator eventAggregator, Func<TeapotViewModel> teapotBuilder, IScriptRunner script, ProjectManager manager)//, Func<ProjectExplorerViewModel> projectExplorerViewModelBuilder, Func<EventAggregatorDebugViewModel> eventsDebugBuilder,)
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
                                    Action = () => _eventAggregator.Publish(new OpenProjectDialog())
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
                    Header = "_BUILD",
                    Items = new BindableCollection<MenuItemViewModel> {
                        new MenuItemViewModel {
                            Header = "_Build Project",
                            Action = () => script.Execute(manager.Project)
                        },
                        new MenuItemViewModel {
                            Header = "Teapot",
                            Action = () => _eventAggregator.Publish(new AddTabViewCommand {Model = teapotBuilder()})
                        },

                    }
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