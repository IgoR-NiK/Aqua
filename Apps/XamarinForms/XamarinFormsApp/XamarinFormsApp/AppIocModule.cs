using Aqua.Core.Ioc;
using Aqua.XamarinForms.Core.Services;
using DryIoc;
using XamarinFormsApp.ViewModels;
using XamarinFormsApp.Views;

namespace XamarinFormsApp
{
    public class AppIocModule : IIocModule
    {
        public void RegisterTypes(IRegistrator container)
        {
            // View Models
            container.Register<ErrorPopupViewModel>();
            container.Register<ExceptionViewModel>();
            container.Register<LoginViewModel>();
            container.Register<LoginTwoViewModel>();
            container.Register<MainViewModel>();
            container.Register<SecondViewModel>();
            
            // Views
            container.Register<ErrorPopupView>();
            container.Register<ExceptionView>();
            container.Register<LoginView>();
            container.Register<LoginTwoView>();
            container.Register<MainView>();
            container.Register<SecondView>();
            
            // Navigation
            container.Register<INavigationModule, AppNavigationModule>();
            
            // App
            container.Register<App>();
        }
    }
}