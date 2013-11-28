using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Autofac.Core;
using Caliburn.Micro;
using OpenCAD.Desktop.ViewModels;
using IContainer = Autofac.IContainer;

namespace OpenCAD.Desktop
{

    public class BootStrapper : AutofacBootstrapper<ShellViewModel>
    {
        protected override void ConfigureBootstrapper()
        {
            base.ConfigureBootstrapper();
            AutoSubscribeEventAggegatorHandlers = true;
            ConventionManager.AddElementConvention<UIElement>(UIElement.VisibilityProperty, "Visibility", "VisibilityChanged");
            var baseBindProperties = ViewModelBinder.BindProperties;
            ViewModelBinder.BindProperties =
                (frameWorkElements, viewModel) =>
                {
                    BindVisiblityProperties(frameWorkElements, viewModel);
                    return baseBindProperties(frameWorkElements, viewModel);
                };
            var baseBindActions = ViewModelBinder.BindActions;
            ViewModelBinder.BindActions =
                (frameWorkElements, viewModel) =>
                {
                    BindVisiblityProperties(frameWorkElements, viewModel);
                    return baseBindActions(frameWorkElements, viewModel);
                };
        }

        private void BindVisiblityProperties(IEnumerable<FrameworkElement> frameWorkElements, Type viewModel)
        {
            foreach (var frameworkElement in frameWorkElements)
            {
                var propertyName = frameworkElement.Name + "IsVisible";
                var property = viewModel.GetPropertyCaseInsensitive(propertyName);
                if (property != null)
                {
                    var convention = ConventionManager
                        .GetElementConvention(typeof(FrameworkElement));
                    ConventionManager.SetBindingWithoutBindingOverwrite(
                        viewModel,
                        propertyName,
                        property,
                        frameworkElement,
                        convention,
                        convention.GetBindableProperty(frameworkElement));
                }
            }
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            //builder.RegisterType<ProjectManager>().AsSelf().SingleInstance();
        }
    }

    public class AutofacBootstrapper<TRootViewModel> : Bootstrapper<TRootViewModel>
    {

        protected IContainer Container { get; private set; }

        public bool EnforceNamespaceConvention { get; set; }

        public bool AutoSubscribeEventAggegatorHandlers { get; set; }

        public Type ViewModelBaseType { get; set; }

        public Func<IWindowManager> CreateWindowManager { get; set; }

        public Func<IEventAggregator> CreateEventAggregator { get; set; }

        protected override void Configure()
        {

            ConfigureBootstrapper();

            if (CreateWindowManager == null)
                throw new ArgumentNullException("CreateWindowManager");

            if (CreateEventAggregator == null)
                throw new ArgumentNullException("CreateEventAggregator");

            var builder = new ContainerBuilder();

            //  register view models
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                //  must be a type that ends with ViewModel
              .Where(type => type.Name.EndsWith("ViewModel"))
                //  must be in a namespace ending with ViewModels
              .Where(type => !(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace.EndsWith("ViewModels"))
                //  must implement INotifyPropertyChanged (deriving from PropertyChangedBase will statisfy this)
              .Where(type => type.GetInterface(typeof(INotifyPropertyChanged).Name) != null)
                //  registered as self
              .AsSelf()
                //  always create a new one
              .InstancePerDependency();

            //  register views
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())         //  must be a type that ends with View
              .Where(type => type.Name.EndsWith("View"))
                //  must be in a namespace that ends in Views
              .Where(type => !(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace.EndsWith("Views"))
                //  registered as self
              .AsSelf()
                //  always create a new one
              .InstancePerDependency();

            //  register the single window manager for this container
            builder.Register<IWindowManager>(c => CreateWindowManager()).InstancePerLifetimeScope();
            //  register the single event aggregator for this container
            builder.Register<IEventAggregator>(c => CreateEventAggregator()).InstancePerLifetimeScope();

            if (AutoSubscribeEventAggegatorHandlers)
                builder.RegisterModule<EventAggregationAutoSubscriptionModule>();

            ConfigureContainer(builder);
            Container = builder.Build();
        }

        protected override object GetInstance(Type service, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                object obj;
                if (Container.TryResolve(service, out obj))
                    return obj;
            }
            else
            {
                object obj;
                if (Container.TryResolveNamed(key, service, out obj))
                    return obj;
            }
            throw new Exception(string.Format("Could not locate any instances of contract {0}.", key ?? service.Name));
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Container.Resolve(typeof(IEnumerable<>).MakeGenericType(new[] { service })) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            Container.InjectProperties(instance);
        }

        protected virtual void ConfigureBootstrapper()
        {
            EnforceNamespaceConvention = true;
            AutoSubscribeEventAggegatorHandlers = false;
            ViewModelBaseType = typeof(INotifyPropertyChanged);
            CreateWindowManager = () => (IWindowManager)new WindowManager();
            CreateEventAggregator = () => (IEventAggregator)new EventAggregator();
        }

        protected virtual void ConfigureContainer(ContainerBuilder builder)
        {
        }
    }
    public class EventAggregationAutoSubscriptionModule : Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration)
        {
            registration.Activated += OnComponentActivated;
        }

        private static void OnComponentActivated(object sender, ActivatedEventArgs<object> e)
        {
            if (e == null)
                return;
            var handle = e.Instance as IHandle;
            if (handle == null)
                return;
            e.Context.Resolve<IEventAggregator>().Subscribe(handle);
        }
    }
}
