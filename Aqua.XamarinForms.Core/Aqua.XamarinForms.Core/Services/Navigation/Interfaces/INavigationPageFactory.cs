using Aqua.Core.Ioc;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services
{
    public interface INavigationPageFactory : IResolvable
    {
        NavigationPage Create(Page page);
    }
}