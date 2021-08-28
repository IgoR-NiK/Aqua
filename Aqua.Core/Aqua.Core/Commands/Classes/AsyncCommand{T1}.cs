using System;
using System.Threading.Tasks;

namespace Aqua.Core.Commands
{
    public class AsyncCommand<T> : AquaCommandBase, IAsyncCommand
    {
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;
        
        public AsyncCommand() { }
        
        public AsyncCommand(Func<T, Task> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute)
            : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public async Task ExecuteAsync(T parameter)
        {
            if (!CanExecute(parameter))
                return;
            
            IsExecuting = true;

            await (_execute ?? ExecuteInternal).Invoke(parameter);

            IsExecuting = false;
        }
        
        public bool CanExecute(T parameter)
            => !IsExecuting && (_canExecute ?? CanExecuteInternal).Invoke(parameter);
        
        protected virtual Task ExecuteInternal(T parameter) => Task.CompletedTask;

        protected virtual bool CanExecuteInternal(T parameter) => true;

        private protected sealed override async void ExecuteCore(object parameter)
        {
            if (IsValidParameter<T>(parameter))
                await ExecuteAsync((T)parameter);
        }

        private protected sealed override bool CanExecuteCore(object parameter)
            => IsValidParameter<T>(parameter) && CanExecute((T)parameter);
    }
}