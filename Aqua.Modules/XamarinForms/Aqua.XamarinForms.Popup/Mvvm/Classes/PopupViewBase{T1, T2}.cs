using Aqua.Core.Mvvm;
using Aqua.Core.Utils;

namespace Aqua.XamarinForms.Popup
{
    public abstract class PopupViewBase<TViewModel, TParam> : PopupViewBase<TViewModel>, IView<TViewModel, TParam>
        where TViewModel : class, IViewModel, IWithInit<TParam>
    {
        public abstract void Init(TParam param);
    }
}