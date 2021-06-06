using System;
using System.Reflection;

using Aqua.Core.Utils;
using Aqua.XamarinForms.Autofac.Helpers;
using Aqua.XamarinForms.Core.Mvvm;
using Aqua.XamarinForms.Core.Services.Navigation;

using Autofac;
using Autofac.Core;

using Xamarin.Forms;

namespace Aqua.XamarinForms.Autofac
{
    public class AquaApplication : Application
    {
        public Assembly[] AssembliesForSearch { get; set; }

        public bool UseAutoRegistrationServices { get; set; } = true;
        
        protected void Run<TViewModel>() 
            where TViewModel : ViewModelBase
        {
            Run<TViewModel>(Array.Empty<IModule>());
        }

        protected void Run<TViewModel>(params IModule[] modules) 
            where TViewModel : ViewModelBase
        {
            var containerBuilder = new ContainerBuilder();
            
            modules.ForEach(it => containerBuilder.RegisterModule(it));
            ContainerBuilderConfiguring(containerBuilder);
            AutoRegistrar.RegistrationServices(containerBuilder, UseAutoRegistrationServices, AssembliesForSearch);

            var container = containerBuilder.Build();
            ContainerConfiguring(container);

            var navigationServiceConfigurator = container.Resolve<INavigationServiceConfigurator>();
            NavigationServiceConfiguring(navigationServiceConfigurator);

            var navigationService = container.Resolve<INavigationService>();
            navigationService.SetMainView<TViewModel>();
        }

        protected virtual void ContainerBuilderConfiguring(ContainerBuilder containerBuilder)
        {
        }

        protected virtual void ContainerConfiguring(IContainer container)
        {
        }

        protected virtual void NavigationServiceConfiguring(INavigationServiceConfigurator configurator)
        {
        }
    }
}