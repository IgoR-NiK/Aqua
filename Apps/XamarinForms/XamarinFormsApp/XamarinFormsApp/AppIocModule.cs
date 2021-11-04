using Aqua.Core.Ioc;
using Aqua.XamarinForms.Core.Services;
using DryIoc;
using XamarinFormsApp.ViewModels;
using XamarinFormsApp.Views;

namespace XamarinFormsApp
{
    public class AppIocModule : IIocModule
    {
        public void RegisterTypes(IRegistrator registrator)
        {
            // View Models
            registrator.Register<ErrorPopupViewModel>();
            registrator.Register<ExceptionViewModel>();
            registrator.Register<LoginViewModel>();
            registrator.Register<LoginTwoViewModel>();
            registrator.Register<MainViewModel>();
            registrator.Register<SecondViewModel>();
            
            // Views
            registrator.Register<ErrorPopupView>();
            registrator.Register<ExceptionView>();
            registrator.Register<LoginView>();
            registrator.Register<LoginTwoView>();
            registrator.Register<MainView>();
            registrator.Register<SecondView>();
            
            // Navigation
            registrator.Register<INavigationModule, AppNavigationModule>();
            
            // App
            registrator.Register<App>();
        }
    }
}