using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aqua.Core.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SecondView : ContentPage, IView
    {
        public SecondView()
        {
            InitializeComponent();
        }
    }
}