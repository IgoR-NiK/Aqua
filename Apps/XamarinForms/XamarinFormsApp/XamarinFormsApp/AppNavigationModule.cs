using Aqua.XamarinForms.Core.Services;
using XamarinFormsApp.ViewModels;
using XamarinFormsApp.Views;

namespace XamarinFormsApp
{
    public class AppNavigationModule : INavigationModule
    {
        public void Map(INavigationMapper navigationMapper)
        {
            navigationMapper.Map<ErrorPopupViewModel, ErrorPopupView>();
            navigationMapper.Map<ExceptionViewModel, ExceptionView>();
            navigationMapper.Map<LoginViewModel, LoginView>();
            navigationMapper.Map<LoginTwoViewModel, LoginTwoView>();
            navigationMapper.Map<MainViewModel, MainView>();
            navigationMapper.Map<SecondViewModel, SecondView>();
        }
    }
}