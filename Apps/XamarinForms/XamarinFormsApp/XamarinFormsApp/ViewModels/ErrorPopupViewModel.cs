using Aqua.Core.Commands;
using Aqua.Core.Mvvm;
using Aqua.Core.Utils;
using Aqua.XamarinForms.Core.Services;

namespace XamarinFormsApp.ViewModels
{
    public class ErrorPopupViewModel : ViewModelBase, IWithInit<string>
    {
        private string _error;
        public string Error
        {
            get => _error;
            set => SetProperty(ref _error, value);
        }
        
        public AsyncCommand OkCommand { get; }

        public void Init(string param)
        {
            Error = param;
        }

        public ErrorPopupViewModel(INavigationService navigationService)
        {
            OkCommand = new AsyncCommand(_ => navigationService.CloseAsync(this));
        }
    }
}