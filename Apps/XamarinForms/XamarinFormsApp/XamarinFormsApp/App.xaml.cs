using Aqua.XamarinForms.Autofac;

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
            
            Run<MainViewModel>();
        }
    }
}