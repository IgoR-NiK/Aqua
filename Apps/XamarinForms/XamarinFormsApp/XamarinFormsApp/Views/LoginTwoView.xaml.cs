using Aqua.Core.IoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginTwoView : ContentPage, IResolvable
    {
        public LoginTwoView()
        {
            InitializeComponent();
        }
    }
}