using System;
using System.Windows.Controls;
using Aqua.Core.Mvvm;

namespace Aqua.Wpf.Core.Services
{
    public interface INavigationMapper
    {
        void Map<TViewModel, TView>()
            where TViewModel : ViewModelBase
            where TView : UserControl;
        
        Type GetViewTypeFor<TViewModel>() 
            where TViewModel : ViewModelBase;

        Type GetViewModelTypeFor<TView>(TView view)
            where TView : UserControl;
    }
}