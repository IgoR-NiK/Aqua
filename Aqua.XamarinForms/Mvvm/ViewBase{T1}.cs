using Aqua.Core.Mvvm;

namespace Aqua.XamarinForms.Mvvm
{
    public abstract class ViewBase<TViewModel> : ViewBase, IView<TViewModel>
        where TViewModel : class, IViewModel
    {
        public TViewModel ViewModel => BindingContext as TViewModel;
    }
}