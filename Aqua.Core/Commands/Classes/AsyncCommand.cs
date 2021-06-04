using System;
using System.Threading.Tasks;

namespace Aqua.Core.Commands
{
    public class AsyncCommand : AquaCommandBase
    {
        private readonly Func<object, Task> _execute;
        
        public AsyncCommand() { }
        
        public AsyncCommand(Func<object, Task> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public AsyncCommand(Func<Task> execute)
            : this(_ => execute())
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
        }

        public AsyncCommand(Func<object, Task> execute, Func<object, bool> canExecute)
            : base(canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
            : this(_ => execute(), _ => canExecute())
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }

        private protected sealed override async void ExecuteCore(object parameter)
            => await ExecuteAsync(parameter);

        public async Task ExecuteAsync(object parameter = null)
        {
            if (!CanExecute(parameter))
                return;

            IsExecuting = true;

            await (_execute ?? ExecuteInternal).Invoke(parameter);

            IsExecuting = false;
        }

        protected virtual Task ExecuteInternal(object parameter) => Task.CompletedTask;
    }
}