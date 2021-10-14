using Aqua.Core.Ioc;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services
{
    public interface INavigationViewProvider : IResolvable
    {
        NavigationPage NavigationView { get; }
    }
}