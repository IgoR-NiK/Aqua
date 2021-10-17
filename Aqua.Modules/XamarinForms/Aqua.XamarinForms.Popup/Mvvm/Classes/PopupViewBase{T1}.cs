using Aqua.Core.Mvvm;

namespace Aqua.XamarinForms.Popup
{
    public abstract class PopupViewBase<TViewModel> : PopupViewBase, IView<TViewModel>
        where TViewModel : class, IViewModel
    {
        public TViewModel ViewModel => BindingContext as TViewModel;
    }
}