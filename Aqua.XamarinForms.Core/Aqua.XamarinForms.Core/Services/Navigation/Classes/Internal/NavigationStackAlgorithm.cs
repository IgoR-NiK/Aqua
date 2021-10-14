using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Core.Mvvm;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services
{
    internal sealed class NavigationStackAlgorithm : StackAlgorithmBase
    {
        private INavigationViewProvider NavigationViewProvider { get; }
        
        public NavigationStackAlgorithm(INavigationViewProvider navigationViewProvider)
        {
            NavigationViewProvider = navigationViewProvider;
        }
        
        public override IReadOnlyList<ViewModelBase> GetStack()
            => NavigationViewProvider.NavigationView.Navigation.NavigationStack
                .Select(it => (ViewModelBase)it.BindingContext)
                .ToList();

        public override async Task NavigateToAsync(Page page, bool withAnimation = true)
            => await NavigationViewProvider.NavigationView.PushAsync(page, withAnimation);

        public override async Task CloseAsync(bool withAnimation = true)
            => await NavigationViewProvider.NavigationView.PopAsync(withAnimation);

        public override async Task<Page> RemoveAsync(int index, bool withAnimation = true)
            => await Task.Run(() =>
            {
                var view = NavigationViewProvider.NavigationView.Navigation.NavigationStack[index];
                NavigationViewProvider.NavigationView.Navigation.RemovePage(view);

                return view;
            });
    }
}