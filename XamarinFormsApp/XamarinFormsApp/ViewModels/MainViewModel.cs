using Aqua.Core.Commands;
using Aqua.XamarinForms.Mvvm;
using Aqua.XamarinForms.Services.Navigation;

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
        
        private int _age;
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }
        
        public AquaCommand TestCommand { get; }
        
        public MainViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
        }
    }
}