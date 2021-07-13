using System;

using Aqua.Core.Mvvm;

using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services.Navigation
{
    public class DefaultNavigationServiceConfigurator : NavigationServiceConfiguratorBase
    {
        public override void Map<TViewModel, TView>()
        {
            if (typeof(TViewModel).GetConstructor(Type.EmptyTypes) == null)
                throw new InvalidOperationException($"{typeof(TViewModel).Name} must have a parameterless constructor");
            
            if (typeof(TView).GetConstructor(Type.EmptyTypes) == null)
                throw new InvalidOperationException($"{typeof(TView).Name} must have a parameterless constructor");
            
            ViewModelTypeToViewTypeMap.Add(typeof(TViewModel), typeof(TView));
            ViewTypeToViewModelTypeMap.Add(typeof(TView), typeof(TViewModel));
        }

        public override void Map(Type viewModelType, Type viewType)
        {
            if (!typeof(ViewModelBase).IsAssignableFrom(viewModelType))
                throw new ArgumentException($"{viewModelType.Name} must be inherited from the {nameof(ViewModelBase)}");
            
            if (viewModelType.GetConstructor(Type.EmptyTypes) == null)
                throw new InvalidOperationException($"{viewModelType.Name} must have a parameterless constructor");
            
            if (!typeof(Page).IsAssignableFrom(viewType))
                throw new ArgumentException($"{viewType.Name} must be inherited from the {nameof(Page)}");
            
            if (viewType.GetConstructor(Type.EmptyTypes) == null)
                throw new InvalidOperationException($"{viewType.Name} must have a parameterless constructor");
            
            ViewModelTypeToViewTypeMap.Add(viewModelType, viewType);
            ViewTypeToViewModelTypeMap.Add(viewType, viewModelType);
        }

        protected override Func<Type, bool> ViewModelPredicate => 
            it => typeof(ViewModelBase).IsAssignableFrom(it) 
                  && !it.IsAbstract 
                  && it.GetConstructor(Type.EmptyTypes) != null;

        protected override Func<Type, bool> ViewPredicate =>
            it => typeof(Page).IsAssignableFrom(it)
                  && !it.IsAbstract
                  && it.GetConstructor(Type.EmptyTypes) != null;
    }
}