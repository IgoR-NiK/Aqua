using System;

using Aqua.XamarinForms.Mvvm;
using Aqua.XamarinForms.Services.Navigation.Classes;

namespace Aqua.XamarinForms.Autofac.Services.Navigation.Classes
{
    public class AutofacNavigationServiceConfigurator : DefaultNavigationServiceConfigurator
    {
        public override void Map<TViewModel, TView>()
        {
            ViewModelTypeToViewTypeMap.Add(typeof(TViewModel), typeof(TView));
            ViewTypeToViewModelTypeMap.Add(typeof(TView), typeof(TViewModel));
        }

        protected override Func<Type, bool> ViewModelPredicate => 
            it => typeof(ViewModelBase).IsAssignableFrom(it) 
                  && !it.IsAbstract;
    }
}