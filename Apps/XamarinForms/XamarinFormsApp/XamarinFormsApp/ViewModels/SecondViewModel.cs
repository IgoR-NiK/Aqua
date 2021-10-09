using System.Threading;
using System.Threading.Tasks;
using Aqua.Core.Commands;
using Aqua.Core.Mvvm;
using Aqua.Core.Utils;
using Aqua.XamarinForms.Core.Services.Navigation;

namespace XamarinFormsApp.ViewModels
{
    public class SecondViewModel : ViewModelBase, IWithInit<string>
    {
        private INavigationService NavigationService { get; }
        
        private AsyncCommand<string> InitAsyncCommand { get; }
        public AsyncCommand GoToExceptionCommand { get; }
        public AsyncCommand CloseCommand { get; }
        
        public SecondViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            
            // Устанавливаем команду для асинхронной инициализации
            InitAsyncCommand = new AsyncCommand<string>(InitAsync);
            
            GoToExceptionCommand = new AsyncCommand(_ => navigationService.NavigateToAsync<ExceptionViewModel>());
            CloseCommand = new AsyncCommand(_ =>
            {
                // Не забываем отменить асинхронную инициализацию при закрытии страницы
                // Вдруг она еще не отработала
                InitAsyncCommand.Cancel();
                return navigationService.CloseAsync(this);
            });
        }
        
        public void Init(string param)
        {
            // Обратить внимание запуск в методе void и без await!
            // Это норм - обработка исключения внутри команды
            InitAsyncCommand.ExecuteAsync(param);
        }

        // Что-то долгое на протяжении 5 секунд перед показом сообщения
        // Обратить внимание - если вернуться назад, то его не будет, т.к. отменили при закрытии
        private Task InitAsync(string param, CancellationToken token)
        {
            return Task.Run(async () =>
            {
                for (var i = 0; i < 50; i++)
                    await Task.Delay(100, token);

                await NavigationService.NavigateToAsync<ErrorPopupViewModel, string>(param, StackType.Popup);
            }, token);
        }
    }
}