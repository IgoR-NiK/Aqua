using Aqua.Core.Mvvm;
using DryIoc;

namespace Aqua.Core.Services
{
    public class DefaultViewProvider<TViewModel> : IViewProvider<TViewModel>
        where TViewModel : IViewModel
    {
        private IResolver Resolver { get; }
        
        private INavigationMappingTypeStorage NavigationMappingTypeStorage { get; }

        public DefaultViewProvider(IResolver resolver, INavigationMappingTypeStorage navigationMappingTypeStorage)
        {
            Resolver = resolver;
            NavigationMappingTypeStorage = navigationMappingTypeStorage;
        }

        public IView GetView(TViewModel viewModel)
        {
            var viewType = NavigationMappingTypeStorage.Get<TViewModel>();
            return (IView)Resolver.Resolve(viewType);
        }
    }
}