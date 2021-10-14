using System;
using Aqua.Core.Mvvm;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services
{
    public interface INavigationMapper
    {
        void Map<TViewModel, TView>()
            where TViewModel : ViewModelBase
            where TView : Page;
        
        Type GetViewTypeFor<TViewModel>() 
            where TViewModel : ViewModelBase;

        Type GetViewModelTypeFor<TView>(TView view)
            where TView : Page;
    }
}