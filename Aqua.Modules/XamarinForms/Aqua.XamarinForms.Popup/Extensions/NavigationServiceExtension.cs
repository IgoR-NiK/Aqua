using System.Threading.Tasks;
using Aqua.XamarinForms.Core.Services;
using Rg.Plugins.Popup.Services;

namespace Aqua.XamarinForms.Popup
{
    public static class NavigationServiceExtension
    {
        public static async Task CloseAllPopupViewsAsync(
            this INavigationService navigationService,
            bool withAnimation = true)
        {
            await navigationService.ExecuteInNavigateSafelyAsync(
                () => PopupNavigation.Instance.PopAllAsync(withAnimation));
        }
    }
}