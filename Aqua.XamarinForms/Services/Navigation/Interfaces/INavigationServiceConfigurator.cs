using System;
using System.Reflection;

using Aqua.Core.IoC;
using Aqua.XamarinForms.Mvvm;

using Xamarin.Forms;

namespace Aqua.XamarinForms.Services.Navigation.Interfaces
{
    [AsSingleInstance]
    public interface INavigationServiceConfigurator : IResolvable
    {
        Assembly[] AssembliesForSearch { get; set; }
        
        bool UseAutoMappingViewModelToView { get; set; }
        
        Func<Page, NavigationPage> NavigationPageCreator { get; set; }
        
        void Map<TViewModel, TView>()
            where TViewModel : ViewModelBase
            where TView : Page;

        void Map(Type viewModelType, Type viewType);

        internal void AutoMappingViewModelToView();
        
        internal Type GetViewTypeFor<TViewModel>() 
            where TViewModel : ViewModelBase;

        internal Type GetViewModelTypeFor(Page view);
    }
}