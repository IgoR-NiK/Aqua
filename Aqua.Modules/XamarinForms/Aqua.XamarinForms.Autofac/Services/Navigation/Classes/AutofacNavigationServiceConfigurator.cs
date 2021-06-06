using System;
using Aqua.XamarinForms.Core.Mvvm;
using Aqua.XamarinForms.Core.Services.Navigation;

using Xamarin.Forms;

namespace Aqua.XamarinForms.Autofac.Services.Navigation.Classes
{
    public class AutofacNavigationServiceConfigurator : DefaultNavigationServiceConfigurator
    {
        public override void Map<TViewModel, TView>()
        {
            ViewModelTypeToViewTypeMap.Add(typeof(TViewModel), typeof(TView));
            ViewTypeToViewModelTypeMap.Add(typeof(TView), typeof(TViewModel));
        }
        
        public override void Map(Type viewModelType, Type viewType)
        {
            if (!typeof(ViewModelBase).IsAssignableFrom(viewModelType))
                throw new ArgumentException($"{viewModelType.Name} must be inherited from the {nameof(ViewModelBase)}");
            
            if (!typeof(Page).IsAssignableFrom(viewType))
                throw new ArgumentException($"{viewType.Name} must be inherited from the {nameof(Page)}");
            
            ViewModelTypeToViewTypeMap.Add(viewModelType, viewType);
            ViewTypeToViewModelTypeMap.Add(viewType, viewModelType);
        }

        protected override Func<Type, bool> ViewModelPredicate => 
            it => typeof(ViewModelBase).IsAssignableFrom(it) 
                  && !it.IsAbstract;

        protected override Func<Type, bool> ViewPredicate =>
            it => typeof(Page).IsAssignableFrom(it) 
                  && !it.IsAbstract;
    }
}