using System;
using Caliburn.Micro;
using OpenCAD.Desktop.Commands;

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
                            Header = "Open Teapot",
                            Action = () => _eventAggregator.Publish(new AddTabViewCommand {Model = teapotBuilder()})
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