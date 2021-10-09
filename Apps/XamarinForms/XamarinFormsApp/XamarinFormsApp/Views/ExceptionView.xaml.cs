using Aqua.Core.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExceptionView : ContentPage, IResolvable
    {
        public ExceptionView()
        {
            InitializeComponent();
        }
    }
}