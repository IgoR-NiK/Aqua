using Aqua.Core.Ioc;
using Aqua.XamarinForms.Core.Services.Navigation;
using DryIoc;

namespace Aqua.XamarinForms.Core
{
    public class AquaXamarinFormsCoreIocModule : IIocModule
    {
        public void RegisterTypes(IRegistrator container)
        {
            // Navigation
            container.Register<INavigationPageFactory, DefaultNavigationPageFactory>(Reuse.Singleton);
            container.Register<INavigationMapper, NavigationMapper>(Reuse.Singleton);
            container.Register<INavigationService, NavigationService>(Reuse.Singleton);
        }
    }
}