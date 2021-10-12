using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services.Navigation
{
    public class DefaultNavigationPageFactory : INavigationPageFactory
    {
        public NavigationPage Create(Page page)
            => new NavigationPage(page);
    }
}