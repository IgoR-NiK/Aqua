using Aqua.Core.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage, IView
    {
        public MainView()
        {
            InitializeComponent();
        }
    }
}