using System;
using Aqua.Core.Ioc;
using Aqua.Core.Mvvm;

namespace Aqua.Core.Services
{
    public interface INavigationMappingTypeStorage : IResolvable
    {
        void Set<TViewModel, TView>()
            where TViewModel : IViewModel
            where TView : IView;

        Type Get<TViewModel>()
            where TViewModel : IViewModel;
    }
}