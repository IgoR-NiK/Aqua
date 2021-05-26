using System.Collections.Generic;

using Aqua.Core.Mvvm;
using Aqua.Core.Utils;
using Aqua.XamarinForms.Services.Navigation;

namespace Aqua.XamarinForms.Mvvm
{
    public abstract class ViewModelBase : RaisableObject, IViewModel
    {
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected ViewModelBase Parent => NavigationService.GetParentFor(this);

        protected ViewModelBase MainParent => NavigationService.GetMainParentFor(this);

        protected IReadOnlyList<ViewModelBase> Children => NavigationService.GetChildrenFor(this);

        protected ViewModelBase Previous => NavigationService.GetPreviousFor(this);

        protected INavigationService NavigationService { get; }

        protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }
    }
}