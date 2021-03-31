using System;

using Aqua.XamarinForms.Mvvm;
using Aqua.XamarinForms.Services.Navigation.Classes;

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

        protected override Func<Type, bool> ViewModelPredicate => 
            it => typeof(ViewModelBase).IsAssignableFrom(it) 
                  && !it.IsAbstract;

        protected override Func<Type, bool> ViewPredicate =>
            it => typeof(Page).IsAssignableFrom(it) 
                  && !it.IsAbstract;
    }
}