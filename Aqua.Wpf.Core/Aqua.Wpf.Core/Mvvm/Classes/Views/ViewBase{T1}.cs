using Aqua.Core.Mvvm;

namespace Aqua.Wpf.Core.Mvvm
{
    public abstract class ViewBase<TViewModel> : ViewBase, IView<TViewModel>
        where TViewModel : class, IViewModel
    {
        public TViewModel ViewModel => DataContext as TViewModel;
    }
}