using Aqua.Core.Ioc;
using Aqua.Core.Mvvm;

namespace Aqua.XamarinForms.Core.Services
{
    public interface IViewModelWrapperStorage : IResolvable
    {
        void Add<TViewModel, TViewModelWrapper>(TViewModel viewModel, TViewModelWrapper viewModelWrapper)
            where TViewModel : ViewModelBase
            where TViewModelWrapper : IViewModelWrapper;
        
        IViewModelWrapper GetValueOrDefault<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase;

        void Clear<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase;

        void ClearAll();
    }
}