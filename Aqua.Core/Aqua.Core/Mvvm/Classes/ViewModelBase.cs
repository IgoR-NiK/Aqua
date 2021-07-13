using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Aqua.Core.Contexts;
using Aqua.Core.Utils;

namespace Aqua.Core.Mvvm
{
    public abstract class ViewModelBase : RaisableObject, IViewModel
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public IContext ParentContext { get; }
        
        public IEnumerable<IContext> ChildrenContexts { get; }

        public IViewModel ParentViewModel => ParentContext as IViewModel;

        public IViewModel MainViewModel => this.FindContext<IViewModel>(it => !(it.ParentContext is IViewModel));

        public IEnumerable<IViewModel> ChildrenViewModels => ChildrenContexts.OfType<IViewModel>();

        public virtual Task OnAppearing() => Task.CompletedTask;

        public virtual Task OnDisappearing() => Task.CompletedTask;
    }
}