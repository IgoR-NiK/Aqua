using Aqua.Core.Interfaces;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage, IResolvable
    {
        public MainView()
        {
            InitializeComponent();
        }
    }
}