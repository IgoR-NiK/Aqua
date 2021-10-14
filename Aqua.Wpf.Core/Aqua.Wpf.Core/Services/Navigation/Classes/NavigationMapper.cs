using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Aqua.Core.Mvvm;

namespace Aqua.Wpf.Core.Services
{
    public sealed class NavigationMapper : INavigationMapper
    {
        private Dictionary<Type, Type> ViewModelTypeToViewTypeMap { get; } = new Dictionary<Type, Type>();
        private Dictionary<Type, Type> ViewTypeToViewModelTypeMap { get; } = new Dictionary<Type, Type>();
        
        public void Map<TViewModel, TView>() 
            where TViewModel : ViewModelBase 
            where TView : UserControl
        {
            ViewModelTypeToViewTypeMap.Add(typeof(TViewModel), typeof(TView));
            ViewTypeToViewModelTypeMap.Add(typeof(TView), typeof(TViewModel));
        }

        public Type GetViewTypeFor<TViewModel>()
            where TViewModel : ViewModelBase
            => ViewModelTypeToViewTypeMap[typeof(TViewModel)];

        public Type GetViewModelTypeFor<TView>(TView view)
            where TView : UserControl
            => ViewTypeToViewModelTypeMap[view.GetType()];
    }
}