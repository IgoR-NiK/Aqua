using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services
{
    internal sealed class DefaultNavigationPageFactory : INavigationPageFactory
    {
        public NavigationPage Create(Page page)
            => new NavigationPage(page);
    }
}