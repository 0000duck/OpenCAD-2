using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autofac;
using Caliburn.Micro;
using IContainer = Autofac.IContainer;

namespace OpenCAD.Desktop.Misc
{
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
                   .Where(type => type.Name.EndsWith("ViewModel"))
                   .Where(type => !(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace.EndsWith("ViewModels"))
                   .Where(type => type.GetInterface(typeof(INotifyPropertyChanged).Name) != null)
                   .AsSelf()
                   .InstancePerDependency();

            //  register views
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                   .Where(type => type.Name.EndsWith("View"))
                   .Where(type => !(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace.EndsWith("Views"))
                   .AsSelf()
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
}