using System;
using System.Windows.Input;

using Aqua.Core.Utils;

namespace Aqua.Core.Commands
{
    public abstract class AquaCommandBase : RaisableObject, IAquaCommand
    {
        private readonly Func<object, bool> _canExecute;
        
        private bool _isExecuting;
        public bool IsExecuting
        {
            get => _isExecuting;
            private protected set => SetProperty(ref _isExecuting, value, it =>
            {
                IsExecutingChanged?.Invoke(it);
                RaiseCanExecuteChanged();
            });
        }
        
        private protected AquaCommandBase() { }

        private protected AquaCommandBase(Func<object, bool> canExecute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public event EventHandler CanExecuteChanged;
        
        public event Action<bool> IsExecutingChanged;

        public bool CanExecute(object parameter = null)
            => !IsExecuting && (_canExecute?.Invoke(parameter) ?? CanExecuteInternal(parameter));

        void ICommand.Execute(object parameter)
        {
            ExecuteCore(parameter);
        }
        
        private protected virtual void ExecuteCore(object parameter) { }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void IsNotExecuting()
        {
            IsExecuting = false;
        }
        
        protected virtual bool CanExecuteInternal(object parameter) => true;
        
        protected static bool IsValidParameter<T>(object o)
        {
            var type = typeof(T);
            return o != null ? o is T : Nullable.GetUnderlyingType(type) != null || !type.IsValueType;
        }
    }
}