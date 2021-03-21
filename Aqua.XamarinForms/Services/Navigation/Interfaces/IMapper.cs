using System;

using Aqua.Core.Interfaces;
using Aqua.XamarinForms.Mvvm;

using Xamarin.Forms;

namespace Aqua.XamarinForms.Services.Navigation.Interfaces
{
    public interface IMapper : IResolvable
    {
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