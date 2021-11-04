using Aqua.Core.Mvvm;
using Aqua.Core.Utils;

namespace Aqua.Wpf.Core.Mvvm
{
    public abstract class ViewBase<TViewModel, TParam> : ViewBase<TViewModel>, IView<TViewModel, TParam>
        where TViewModel : class, IViewModel, IWithInit<TParam>
    {
        public abstract void Init(TParam param);
    }
}