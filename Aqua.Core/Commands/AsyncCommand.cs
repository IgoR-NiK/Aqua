using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Aqua.Core.Utils;

namespace Aqua.Core.Commands
{
    public class AsyncCommand : RaisableObject, ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            private set => SetProperty(ref _isActive, value, () =>
            {
                IsActiveChanged?.Invoke(value);
                ChangeCanExecute();
            });
        }

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

        public event Action<bool> IsActiveChanged;

        public bool CanExecute(object parameter)
        {
            return !IsActive && (_canExecute?.Invoke(parameter) ?? true);
        }

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            IsActive = true;

            await _execute(parameter);

            IsActive = false;
        }

        public void ChangeCanExecute()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void IsNotActive()
        {
            IsActive = false;
        }
    }
}