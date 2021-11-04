using Aqua.Core.Ioc;
using Aqua.Core.Mvvm;

namespace Aqua.Core.Services
{
    public interface INavigationMapper : IResolvable
    {
        INavigationMapper Map<TViewModel, TView>()
            where TViewModel : IViewModel
            where TView : IView;
    }
}