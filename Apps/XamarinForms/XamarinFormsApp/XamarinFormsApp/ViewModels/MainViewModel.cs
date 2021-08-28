using System.Threading.Tasks;
using Aqua.Core.Commands;
using Aqua.Core.Mvvm;
using Aqua.Core.Utils;

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
        
        public AsyncCommand TestCommand { get; }

        public MainViewModel()
        {
            TestCommand = new AsyncCommand(() => Task.Delay(10000), () => !Text.IsNullOrEmpty())
                .ListenOn(this, it => it.Text);
        }
    }
}