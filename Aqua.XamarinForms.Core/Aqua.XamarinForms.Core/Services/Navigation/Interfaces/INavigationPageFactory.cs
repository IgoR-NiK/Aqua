using Aqua.Core.Ioc;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services.Navigation
{
    public interface INavigationPageFactory : IResolvable
    {
        NavigationPage Create(Page page);
    }
}