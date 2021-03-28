using Aqua.XamarinForms.Mvvm;
using Aqua.XamarinForms.Services.Navigation;

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