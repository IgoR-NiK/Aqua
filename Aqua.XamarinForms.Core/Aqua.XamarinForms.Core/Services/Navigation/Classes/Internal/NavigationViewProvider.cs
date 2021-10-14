using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services
{
    internal sealed class NavigationViewProvider : INavigationViewProvider
    {
        public NavigationPage NavigationView 
            => Application.Current.MainPage is FlyoutPage flyoutPage
                ? (NavigationPage)flyoutPage.Detail
                : (NavigationPage)Application.Current.MainPage;
    }
}