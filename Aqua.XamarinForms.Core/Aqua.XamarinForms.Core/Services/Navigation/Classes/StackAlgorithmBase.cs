using System.Collections.Generic;
using System.Threading.Tasks;
using Aqua.Core.Mvvm;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services
{
    public abstract class StackAlgorithmBase : IStackAlgorithm
    {
        public bool TryGetFromStack<TViewModel>(TViewModel viewModel, out IReadOnlyList<ViewModelBase> stack, out int index)
            where TViewModel : ViewModelBase
        {
            stack = GetStack();
            index = ((List<ViewModelBase>)stack).IndexOf(viewModel);

            return index >= 0;
        }
        
        public abstract IReadOnlyList<ViewModelBase> GetStack();
        
        public abstract Task NavigateToAsync(Page page, bool withAnimation = true);
        
        public abstract Task CloseAsync(bool withAnimation = true);
        
        public abstract Task<Page> RemoveAsync(int index, bool withAnimation = true);
    }
}