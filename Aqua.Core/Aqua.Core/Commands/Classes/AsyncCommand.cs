using System;
using System.Threading.Tasks;

namespace Aqua.Core.Commands
{
    public class AsyncCommand : AquaCommandBase, IAsyncCommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        
        public AsyncCommand() { }
        
        public AsyncCommand(Func<Task> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
            : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }
        
        public async Task ExecuteAsync()
        {
            if (!CanExecute())
                return;

            IsExecuting = true;

            await (_execute ?? ExecuteInternal).Invoke();

            IsExecuting = false;
        }
        
        public bool CanExecute()
            => !IsExecuting && (_canExecute ?? CanExecuteInternal).Invoke();
        
        protected virtual Task ExecuteInternal() => Task.CompletedTask;

        protected virtual bool CanExecuteInternal() => true;
        
        private protected sealed override async void ExecuteCore(object parameter)
            => await ExecuteAsync();

        private protected sealed override bool CanExecuteCore(object parameter)
            => CanExecute();
    }
}