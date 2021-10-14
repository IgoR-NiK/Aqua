using Aqua.Core.Utils;
using Aqua.XamarinForms.Core.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFormsApp.ViewModels;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinFormsApp
{
    public partial class App : Application, IAquaApplication
    {
        public App(INavigationService navigationService)
        {
            InitializeComponent();
            
            navigationService.SetMainView<MainViewModel>();
        }
    }
}