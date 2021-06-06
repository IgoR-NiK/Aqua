using Aqua.XamarinForms.Core.Mvvm;
using Aqua.XamarinForms.Core.Services.Navigation;

namespace XamarinFormsApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
        }
    }
}