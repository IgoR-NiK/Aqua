using System;
using System.Collections.Generic;
using Aqua.Core.Mvvm;

namespace Aqua.Core.Services
{
    public class DefaultNavigationMappingTypeStorage : INavigationMappingTypeStorage
    {
        private Dictionary<Type, Type> ViewModelTypeToViewTypeMap { get; } = new Dictionary<Type, Type>();

        public void Set<TViewModel, TView>()
            where TViewModel : IViewModel
            where TView : IView
            => ViewModelTypeToViewTypeMap[typeof(TViewModel)] = typeof(TView);

        public Type Get<TViewModel>()
            where TViewModel : IViewModel
            => ViewModelTypeToViewTypeMap[typeof(TViewModel)];
    }
}