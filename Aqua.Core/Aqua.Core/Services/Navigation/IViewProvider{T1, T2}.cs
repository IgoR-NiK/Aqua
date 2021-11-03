using Aqua.Core.Ioc;
using Aqua.Core.Mvvm;
using Aqua.Core.Utils;

namespace Aqua.Core.Services
{
    public interface IViewProvider<in TViewModel, in TParam> : IResolvable
        where TViewModel : class, IViewModel, IWithInit<TParam>
    {
        IView GetView(TViewModel viewModel, TParam param);
    }
}