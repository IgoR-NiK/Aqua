using System;
using System.Reflection;
using Aqua.XamarinForms.Autofac;
using Aqua.XamarinForms.Services.Navigation.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFormsApp.ViewModels;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace XamarinFormsApp
{
    public partial class App : AquaApplication
    {
        public App()
        {
            InitializeComponent();

            AssembliesForSearch = new[] { Assembly.GetExecutingAssembly() };
            
            Run<MainViewModel>();
        }

        protected override void NavigationServiceConfiguring(INavigationServiceConfigurator configurator)
        {
            configurator.AssembliesForSearch = new[] { Assembly.GetExecutingAssembly() };
        }
    }
}