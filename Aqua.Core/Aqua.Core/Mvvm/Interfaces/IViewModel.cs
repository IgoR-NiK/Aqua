using System.Collections.Generic;
using System.Threading.Tasks;

using Aqua.Core.Contexts;

namespace Aqua.Core.Mvvm
{
    public interface IViewModel : IViewModelContext
    {
        IViewModel ParentViewModel { get; }
        
        IViewModel MainViewModel { get; }
        
        IEnumerable<IViewModel> ChildrenViewModels { get; }
        
        Task OnAppearing();

        Task OnDisappearing();
    }
}