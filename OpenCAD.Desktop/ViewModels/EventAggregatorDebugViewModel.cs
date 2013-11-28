using Caliburn.Micro;

namespace OpenCAD.Desktop.ViewModels
{
    public class EventAggregatorDebugViewModel : AvalonViewModelBaseBase, IHandle<object>
    {
        public BindableCollection<object> Events { get; set; }

        public EventAggregatorDebugViewModel(IEventAggregator eventAggregator)
        {
            Title = "Events Debug";
            Events = new BindableCollection<object>();
            eventAggregator.Subscribe(this);
        }

        public void Handle(object message)
        {
            Events.Add(message);
        }
    }
}