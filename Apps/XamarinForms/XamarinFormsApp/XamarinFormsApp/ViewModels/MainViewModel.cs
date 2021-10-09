using Aqua.Core.Commands;
using Aqua.Core.Mvvm;
using Aqua.XamarinForms.Core.Services.Navigation;

namespace XamarinFormsApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
        
        public AsyncCommand GoToSecondCommand { get; }

        public MainViewModel(INavigationService navigationService)
        {
            GoToSecondCommand = new AsyncCommand(_ => navigationService.NavigateToAsync<SecondViewModel, string>(Text));
        }
    }
}