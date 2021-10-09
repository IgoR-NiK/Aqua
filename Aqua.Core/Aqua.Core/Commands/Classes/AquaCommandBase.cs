using System;
using System.Windows.Input;

using Aqua.Core.Utils;

namespace Aqua.Core.Commands
{
    public abstract class AquaCommandBase : RaisableObject, IAquaCommandBase
    {
        private bool _isExecuting;
        public bool IsExecuting
        {
            get => _isExecuting;
            private protected set => SetProperty(ref _isExecuting, value, it =>
            {
                IsExecutingChanged?.Invoke(it);
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            });
        }

        private bool _isFaulted;
        public bool IsFaulted
        {
            get => _isFaulted;
            private protected set => SetProperty(ref _isFaulted, value);
        }
        
        public event EventHandler CanExecuteChanged;

        public event Action CheckCanExecuteFunc;
        
        public event Action<bool> IsExecutingChanged;

        private protected AquaCommandBase() { }

        public void RaiseCanExecuteChanged()
        {
            CheckCanExecuteFunc?.Invoke();
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void IsNotExecuting()
        {
            IsExecuting = false;
        }

        protected static bool IsValidParameter<T>(object o)
        {
            var type = typeof(T);
            return o != null ? o is T : Nullable.GetUnderlyingType(type) != null || !type.IsValueType;
        }

        private protected abstract void ExecuteCore(object parameter);

        private protected abstract bool CanExecuteCore(object parameter);

        private protected abstract bool CanExecuteFunc(object parameter);
        
        void ICommand.Execute(object parameter)
            => ExecuteCore(parameter);

        bool ICommand.CanExecute(object parameter)
            => CanExecuteCore(parameter);

        bool IAquaCommandBase.CanExecuteFunc(object parameter)
            => CanExecuteFunc(parameter);
    }
}