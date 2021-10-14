using Aqua.Core.Ioc;
using Aqua.Core.Services;
using Aqua.XamarinForms.Core.Services;
using DryIoc;

namespace Aqua.XamarinForms.Core
{
    public sealed class AquaXamarinFormsCoreIocModule : IIocModule
    {
        public void RegisterTypes(IRegistrator container)
        {
            // Navigation
            container.Register<IStackAlgorithm, NavigationStackAlgorithm>(Reuse.Singleton, serviceKey: typeof(NavigationStack));
            container.Register<IStackAlgorithm, ModalStackAlgorithm>(Reuse.Singleton, serviceKey: typeof(ModalStack));
            container.Register<IViewModelWrapperStorage, ViewModelWrapperStorage>(Reuse.Singleton);
            container.Register<INavigationViewProvider, NavigationViewProvider>(Reuse.Singleton);
            container.Register<INavigationPageFactory, DefaultNavigationPageFactory>(Reuse.Singleton);
            container.Register<INavigationMapper, NavigationMapper>(Reuse.Singleton);
            container.Register<INavigationService, NavigationService>(Reuse.Singleton);
        }
    }
}