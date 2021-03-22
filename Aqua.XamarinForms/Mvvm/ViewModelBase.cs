using System.Collections.Generic;
using System.Threading.Tasks;

using Aqua.Core.Interfaces;
using Aqua.Core.Utils;
using Aqua.XamarinForms.Services.Navigation;

namespace Aqua.XamarinForms.Mvvm
{
    public abstract class ViewModelBase : RaisableObject, IResolvable
    {
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value, () => RaisePropertyChanged(nameof(IsNotBusy)));
        }

        public bool IsNotBusy => !IsBusy;

        protected ViewModelBase Parent => NavigationService.GetParentFor(this);

        protected ViewModelBase MainParent => NavigationService.GetMainParentFor(this);

        protected IReadOnlyList<ViewModelBase> Children => NavigationService.GetChildrenFor(this);

        protected ViewModelBase Previous => NavigationService.GetPreviousFor(this);

        protected INavigationService NavigationService { get; }

        protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual Task OnAppearing()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnDisappearing()
        {
            return Task.CompletedTask;
        }
    }
}