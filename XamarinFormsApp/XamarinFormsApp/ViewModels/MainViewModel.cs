using System.Collections.Generic;
using Aqua.XamarinForms.Mvvm;
using Aqua.XamarinForms.Services.Navigation;

namespace XamarinFormsApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationService navigationService, IEnumerable<ILogger> loggers) 
            : base(navigationService)
        {
            var x = loggers;
        }
    }
}