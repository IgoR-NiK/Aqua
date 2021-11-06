using Aqua.Core.Ioc;
using Aqua.Core.Mvvm;
using Aqua.Core.Utils;
using DryIoc;

namespace Aqua.Core.Services
{
    public class DefaultViewProvider<TViewModel, TParam> : IViewProvider<TViewModel, TParam>
        where TViewModel : IViewModel, IWithInit<TParam>
    {
        private IResolver Resolver { get; }
        
        private INavigationMappingTypeStorage NavigationMappingTypeStorage { get; }

        public DefaultViewProvider(IResolver resolver, INavigationMappingTypeStorage navigationMappingTypeStorage)
        {
            Resolver = resolver;
            NavigationMappingTypeStorage = navigationMappingTypeStorage;
        }

        public IView GetView(TViewModel viewModel, TParam param)
        {
            var viewType = NavigationMappingTypeStorage.Get<TViewModel>();
            return (IView)Resolver.Resolve(viewType, param);
        }
    }
}