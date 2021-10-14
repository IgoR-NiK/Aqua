using System.Collections.Generic;
using System.Threading.Tasks;
using Aqua.Core.Ioc;
using Aqua.Core.Mvvm;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services
{
    public interface IStackAlgorithm : IResolvable
    {
        IReadOnlyList<ViewModelBase> GetStack();

        bool TryGetFromStack<TViewModel>(TViewModel viewModel, out IReadOnlyList<ViewModelBase> stack, out int index)
            where TViewModel : ViewModelBase;
        
        Task NavigateToAsync(Page page, bool withAnimation = true);
        
        Task CloseAsync(bool withAnimation = true);
        
        Task<Page> RemoveAsync(int index, bool withAnimation = true);
    }
}