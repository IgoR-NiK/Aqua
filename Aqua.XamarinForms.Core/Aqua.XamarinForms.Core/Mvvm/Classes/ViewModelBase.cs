using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Core.Contexts;
using Aqua.Core.Mvvm;
using Aqua.Core.Utils;
using Aqua.XamarinForms.Core.Services.Navigation;

namespace Aqua.XamarinForms.Core.Mvvm
{
    public abstract class ViewModelBase : RaisableObject, IViewModel
    {
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public IContext ParentContext { get; }
        
        public IEnumerable<IContext> ChildrenContexts { get; }

        protected ViewModelBase ParentViewModel => ParentContext as ViewModelBase;

        protected ViewModelBase MainViewModel => this.FindContext<ViewModelBase>(it => !(it.ParentContext is ViewModelBase));

        protected IEnumerable<ViewModelBase> ChildrenViewModels => ChildrenContexts.OfType<ViewModelBase>();

        protected ViewModelBase Previous => NavigationService.GetPreviousFor(this);

        protected INavigationService NavigationService { get; }

        protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual Task OnAppearing() => Task.CompletedTask;

        public virtual Task OnDisappearing() => Task.CompletedTask;
    }
}