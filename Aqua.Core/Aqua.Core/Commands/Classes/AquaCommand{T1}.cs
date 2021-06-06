using System;

namespace Aqua.Core.Commands
{
    public class AquaCommand<T> : AquaCommandBase
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;
        
        public AquaCommand() { }
        
        public AquaCommand(Action<T> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public AquaCommand(Action<T> execute, Func<T, bool> canExecute)
            : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public void Execute(T parameter)
        {
            if (!CanExecute(parameter))
                return;
            
            IsExecuting = true;

            (_execute ?? ExecuteInternal).Invoke(parameter);

            IsExecuting = false;
        }
        
        public bool CanExecute(T parameter)
            => !IsExecuting && (_canExecute ?? CanExecuteInternal).Invoke(parameter);
        
        protected virtual void ExecuteInternal(T parameter) { }

        protected virtual bool CanExecuteInternal(T parameter) => true;

        private protected sealed override void ExecuteCore(object parameter)
        {
            if (IsValidParameter<T>(parameter))
                Execute((T)parameter);
        }

        private protected sealed override bool CanExecuteCore(object parameter)
            => IsValidParameter<T>(parameter) && CanExecute((T)parameter);
    }
}