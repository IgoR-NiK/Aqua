using Aqua.Core.Interfaces;

namespace Aqua.Core.Mvvm
{
    public interface IView<out TViewModel, in TParam> : IView<TViewModel>, IWithInit<TParam>
        where TViewModel : class, IViewModel, IWithInit<TParam>
    {
    }
}