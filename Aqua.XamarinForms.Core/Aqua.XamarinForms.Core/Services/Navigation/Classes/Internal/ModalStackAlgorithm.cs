using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Core.Mvvm;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services
{
    internal sealed class ModalStackAlgorithm : StackAlgorithmBase
    {
        private INavigationViewProvider NavigationViewProvider { get; }
        
        public ModalStackAlgorithm(INavigationViewProvider navigationViewProvider)
        {
            NavigationViewProvider = navigationViewProvider;
        }
        
        public override IReadOnlyList<ViewModelBase> GetStack()
            => NavigationViewProvider.NavigationView.Navigation.ModalStack
                .Select(it => (ViewModelBase)it.BindingContext)
                .ToList();

        public override async Task NavigateToAsync(Page page, bool withAnimation = true)
            => await NavigationViewProvider.NavigationView.Navigation.PushModalAsync(page, withAnimation);

        public override async Task CloseAsync(bool withAnimation = true)
            => await NavigationViewProvider.NavigationView.Navigation.PopModalAsync(withAnimation);

        public override Task<Page> RemoveAsync(int index, bool withAnimation = true)
            => throw new NotImplementedException();
    }
}