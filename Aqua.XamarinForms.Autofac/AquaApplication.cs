using System;

using Aqua.Core.Extensions;
using Aqua.XamarinForms.Mvvm;
using Aqua.XamarinForms.Services.Navigation;
using Aqua.XamarinForms.Services.Navigation.Interfaces;

using Autofac;
using Autofac.Core;

using Xamarin.Forms;

namespace Aqua.XamarinForms.Autofac
{
    public class AquaApplication : Application
    {
        public bool UseAutoRegistrationServices { get; set; }
        
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
            AutoRegistrationServices(containerBuilder);

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

        private void AutoRegistrationServices(ContainerBuilder containerBuilder)
        {
            if (!UseAutoRegistrationServices)
                return;
        }
    }
}