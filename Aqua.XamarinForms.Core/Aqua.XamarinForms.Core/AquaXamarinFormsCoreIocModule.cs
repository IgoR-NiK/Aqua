using Aqua.Core.Ioc;
using Aqua.Core.Services;
using Aqua.XamarinForms.Core.Services;
using DryIoc;

namespace Aqua.XamarinForms.Core
{
    public sealed class AquaXamarinFormsCoreIocModule : IIocModule
    {
        public void RegisterTypes(IRegistrator registrator)
        {
            // Navigation
            registrator.Register<IStackAlgorithm, NavigationStackAlgorithm>(Reuse.Singleton, serviceKey: typeof(NavigationStack));
            registrator.Register<IStackAlgorithm, ModalStackAlgorithm>(Reuse.Singleton, serviceKey: typeof(ModalStack));
            registrator.Register<IViewModelWrapperStorage, ViewModelWrapperStorage>(Reuse.Singleton);
            registrator.Register<INavigationViewProvider, NavigationViewProvider>(Reuse.Singleton);
            registrator.Register<INavigationPageFactory, DefaultNavigationPageFactory>(Reuse.Singleton);
            registrator.Register<INavigationMapper, NavigationMapper>(Reuse.Singleton);
            registrator.Register<INavigationService, NavigationService>(Reuse.Singleton);
        }
    }
}