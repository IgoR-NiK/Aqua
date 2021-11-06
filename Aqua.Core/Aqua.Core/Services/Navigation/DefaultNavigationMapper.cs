using Aqua.Core.Mvvm;

namespace Aqua.Core.Services
{
    public class DefaultNavigationMapper : INavigationMapper
    {
        private INavigationMappingTypeStorage NavigationMappingTypeStorage { get; }
        
        public DefaultNavigationMapper(INavigationMappingTypeStorage navigationMappingTypeStorage)
        {
            NavigationMappingTypeStorage = navigationMappingTypeStorage;
        }

        public INavigationMapper Map<TViewModel, TView>() 
            where TViewModel : IViewModel 
            where TView : IView
        {
            NavigationMappingTypeStorage.Set<TViewModel, TView>();
            return this;
        }
    }
}