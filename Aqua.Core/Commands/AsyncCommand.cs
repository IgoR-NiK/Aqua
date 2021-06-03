using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Aqua.Core.Utils;

namespace Aqua.Core.Commands
{
    public class AsyncCommand : RaisableObject, IAquaCommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;

        private bool _isExecuting;

        public bool IsExecuting
        {
            get => _isExecuting;
            private set => SetProperty(ref _isExecuting, value, it =>
            {
                IsExecutingChanged?.Invoke(it);
                RaiseCanExecuteChanged();
            });
        }

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
            : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
            : this(_ => execute(), _ => canExecute())
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }

        public event EventHandler CanExecuteChanged;

        public event Action<bool> IsExecutingChanged;

        public bool CanExecute(object parameter = null)
        {
            return !IsExecuting && (_canExecute?.Invoke(parameter) ?? CanExecuteInternal(parameter));
        }

        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }
        
        public async Task ExecuteAsync(object parameter = null)
        {
            if (!CanExecute(parameter))
                return;

            IsExecuting = true;

            await (_execute?.Invoke(parameter) ?? ExecuteInternal(parameter));

            IsExecuting = false;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void IsNotExecuting()
        {
            IsExecuting = false;
        }

        protected virtual Task ExecuteInternal(object parameter) => Task.CompletedTask;

        protected virtual bool CanExecuteInternal(object parameter) => true;
    }
}