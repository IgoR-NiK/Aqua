using Aqua.Core.Ioc;
using Aqua.Core.Mvvm;

namespace Aqua.Core.Services
{
    public interface IViewProvider<in TViewModel> : IResolvable
        where TViewModel : IViewModel
    {
        IView GetView(TViewModel viewModel);
    }
}