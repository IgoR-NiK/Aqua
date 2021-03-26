using System;
using System.Reflection;

using Aqua.Core.Attributes;
using Aqua.Core.Interfaces;
using Aqua.XamarinForms.Mvvm;

using Xamarin.Forms;

namespace Aqua.XamarinForms.Services.Navigation.Interfaces
{
    [AsSingleInstance]
    public interface IMapper : IResolvable
    {
        Assembly[] AssembliesForSearch { get; set; }
        
        bool UseAutoMappingViewModelToView { get; set; }
        
        void Map<TViewModel, TView>()
            where TViewModel : ViewModelBase
            where TView : Page, new();

        internal void AutoMappingViewModelToView();
        
        internal Type GetViewTypeFor<TViewModel>() 
            where TViewModel : ViewModelBase;

        internal Type GetViewModelTypeFor(Page view);
    }
}