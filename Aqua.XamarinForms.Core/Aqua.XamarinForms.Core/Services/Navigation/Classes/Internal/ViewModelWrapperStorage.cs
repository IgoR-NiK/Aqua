using System.Collections.Generic;
using Aqua.Core.Mvvm;

namespace Aqua.XamarinForms.Core.Services
{
    internal sealed class ViewModelWrapperStorage : IViewModelWrapperStorage
    {
        private Dictionary<ViewModelBase, IViewModelWrapper> ViewModelWrappers { get; } = new Dictionary<ViewModelBase, IViewModelWrapper>();

        public void Add<TViewModel, TViewModelWrapper>(TViewModel viewModel, TViewModelWrapper viewModelWrapper)
            where TViewModel : ViewModelBase where TViewModelWrapper : IViewModelWrapper
        {
            ViewModelWrappers.Add(viewModel, viewModelWrapper);
        }

        public IViewModelWrapper GetValueOrDefault<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase
        {
            return ViewModelWrappers.GetValueOrDefault(viewModel);
        }

        public void Clear<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase
        {
            if (ViewModelWrappers.TryGetValue(viewModel, out var viewModelWrapper))
            {
                foreach (var children in viewModelWrapper.Children)
                {
                    Clear(children);
                }

                ViewModelWrappers.Remove(viewModel);
            }
        }

        public void ClearAll()
        {
            ViewModelWrappers.Clear();
        }
    }
}