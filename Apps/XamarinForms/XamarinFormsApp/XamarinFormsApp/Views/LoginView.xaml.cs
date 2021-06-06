using Aqua.Core.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage, IResolvable
    {
        public LoginView()
        {
            InitializeComponent();
        }
    }
}