using System;
using System.Collections.Generic;
using System.Windows;
using Autofac;
using Autofac.Core;
using Caliburn.Micro;
using OpenCAD.Desktop.Misc;
using OpenCAD.Desktop.ViewModels;
using OpenCAD.Kernel.Graphics;
using OpenCAD.Kernel.Graphics.OpenGLRenderer;
using OpenCAD.Kernel.Scripting;

namespace OpenCAD.Desktop
{

    public class BootStrapper : AutofacBootstrapper<ShellViewModel>
    {

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<ProjectManager>().AsSelf().SingleInstance();
            builder.RegisterType<OpenGLRenderer>().As<IRenderer>();
            builder.RegisterType<OrthographicCamera>().As<ICamera>();
            builder.RegisterType<ScriptRunner>().As<IScriptRunner>();
        }

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

    }


}
