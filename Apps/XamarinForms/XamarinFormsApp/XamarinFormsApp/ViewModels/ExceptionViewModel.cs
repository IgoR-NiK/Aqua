using System;
using System.Threading;
using System.Threading.Tasks;
using Aqua.Core.Commands;
using Aqua.Core.Mvvm;
using Aqua.XamarinForms.Core.Services;
using Aqua.XamarinForms.Popup;

namespace XamarinFormsApp.ViewModels
{
    public class ExceptionViewModel : ViewModelBase
    {
        private INavigationService NavigationService { get; }
        
        private AsyncCommand InitAsyncCommand { get; }
        public AsyncCommand CloseCommand { get; }
        
        public ExceptionViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            
            // Устанавливаем команду для асинхронной инициализации с обработчиком ошибки
            InitAsyncCommand = new AsyncCommand(InitAsync).WithFaultedHandlerAsync(FaultedHandlerAsync);
            
            // Обратить внимание запуск в конструкторе и без await!
            // Это норм - обработка исключения внутри команды
            InitAsyncCommand.ExecuteAsync();
            
            CloseCommand = new AsyncCommand(_ =>
            {
                // Не забываем отменить асинхронную инициализацию при закрытии страницы
                // Вдруг она еще не отработала
                InitAsyncCommand.Cancel();
                return navigationService.CloseAsync(this);
            });
        }
        
        // Что-то долгое на протяжении 5 секунд перед ошибкой
        // Обратить внимание - если вернуться назад, то ошибки не будет, т.к. отменили при закрытии
        private Task InitAsync(CancellationToken token)
        {
            return Task.Run(async () =>
            {
                for (var i = 0; i < 50; i++)
                    await Task.Delay(100, token);

                throw new Exception("My Exception");
            }, token);
        }

        private async Task FaultedHandlerAsync(Exception exception)
        {
            await NavigationService.NavigateToAsync<ErrorPopupViewModel, string>(
                exception.Message, config => config.In<PopupStack>());
        }
    }
}