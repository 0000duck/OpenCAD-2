using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reactive.Linq;
using Caliburn.Micro;
using OpenCAD.Desktop.Commands;
using OpenCAD.Desktop.Misc;
using OpenCAD.Kernel.Modeling;
using OpenCAD.Kernel.Structure;
using Xceed.Wpf.AvalonDock;

namespace OpenCAD.Desktop.ViewModels
{
    public class ShellViewModel : Conductor<Screen>, IHandle<AddTabViewCommand>, IHandle<AddToolViewCommand>, IHandle<OpenItemCommand>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<IModel, RendererViewModel> _renderBuilder;
        private readonly Func<IProjectItem, TextItemViewModel> _itemViewModelBuilder;
        private readonly Func<PartProjectItem, PartItemViewModel> _partitemViewModelBuilder;
        private readonly ProjectManager _projectManager;
        private PropertyChangedBase _activeDocument;

        public PropertyChangedBase ActiveDocument
        {
            get { return _activeDocument; }
            set
            {
                if (Equals(value, _activeDocument)) return;
                _activeDocument = value;
                NotifyOfPropertyChange(() => ActiveDocument);
            }
        }

        public BindableCollection<PropertyChangedBase> Tabs { get; set; }
        public BindableCollection<PropertyChangedBase> Tools { get; set; }
        public MenuViewModel Menu { get; set; }

        public ShellViewModel(
            IEventAggregator eventAggregator, 
            MenuViewModel menu, 
            ProjectManager projectManager, 
            Func<EventAggregatorDebugViewModel> eventsDebugBuilder,
            Func<IModel, RendererViewModel> renderBuilder, 
            Func<ProjectExplorerViewModel> projectExplorerViewModelBuilder,
            Func<IProjectItem, TextItemViewModel> itemViewModelBuilder,
            Func<PartProjectItem, PartItemViewModel> partitemViewModelBuilder)
        {
            _eventAggregator = eventAggregator;
            _projectManager = projectManager;
            _renderBuilder = renderBuilder;
            _itemViewModelBuilder = itemViewModelBuilder;
            _partitemViewModelBuilder = partitemViewModelBuilder;
            Tabs = new BindableCollection<PropertyChangedBase>
            {

            };
            Tools = new BindableCollection<PropertyChangedBase> {
                projectExplorerViewModelBuilder(),
                eventsDebugBuilder()
            };

            Menu = menu;
            
            InitializeEvents();
        }

        public void Handle(AddTabViewCommand message)
        {
            Tabs.Add(message.Model);
            ActiveDocument = message.Model;
        }

        public void Handle(AddToolViewCommand message)
        {
            Tools.Add(message.Model);
        }

        private void InitializeEvents()
        {
            var tabsChanged = Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(Tabs, "CollectionChanged");
            var toolsChanged = Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(Tools, "CollectionChanged");

            tabsChanged.Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Add).Subscribe(a => _eventAggregator.Publish(new TabAddedEvent { Args = a.EventArgs }));
            tabsChanged.Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Remove).Subscribe(a => _eventAggregator.Publish(new TabRemovedEvent { Args = a.EventArgs }));

            toolsChanged.Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Add).Subscribe(a => _eventAggregator.Publish(new ToolAddedEvent { Args = a.EventArgs }));
            toolsChanged.Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Remove).Subscribe(a => _eventAggregator.Publish(new ToolRemovedEvent { Args = a.EventArgs }));
        }

        public void DocumentClosed(DocumentClosedEventArgs e)
        {
            Tabs.Remove(e.Document.Content as Screen);
            var disposable = e.Document.Content as IDisposable;
            if (disposable != null) disposable.Dispose();
        }

        public void DocumentClosing(DocumentClosingEventArgs e)
        {
            
        }

        public void Handle(OpenItemCommand message)
        {
            if (message.Item is PartProjectItem)
            {
                _eventAggregator.Publish(new AddTabViewCommand { Model = _partitemViewModelBuilder((PartProjectItem)message.Item) });
            }
            else
            {

                _eventAggregator.Publish(new AddTabViewCommand { Model = _itemViewModelBuilder(message.Item) });
            }

           // _eventAggregator.Publish(new AddTabViewCommand { Model = _itemViewModelBuilder(message.Item) });
        }
    }
}
