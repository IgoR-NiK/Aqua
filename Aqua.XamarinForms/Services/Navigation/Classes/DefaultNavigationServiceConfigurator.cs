using System;

using Aqua.XamarinForms.Mvvm;

using Xamarin.Forms;

namespace Aqua.XamarinForms.Services.Navigation.Classes
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