using Aqua.Core.Mvvm;
using Aqua.Core.Utils;

namespace Aqua.XamarinForms.Mvvm
{
    public abstract class ViewBase<TViewModel, TParam> : ViewBase<TViewModel>, IView<TViewModel, TParam>
        where TViewModel : class, IViewModel, IWithInit<TParam>
    {
        public virtual void Init(TParam param) { }
    }
}