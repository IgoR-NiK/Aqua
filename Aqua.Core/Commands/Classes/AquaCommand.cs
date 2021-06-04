using System;

namespace Aqua.Core.Commands
{
    public class AquaCommand : AquaCommandBase
    {
        private readonly Action<object> _execute;
        
        public AquaCommand() { }

        public AquaCommand(Action execute)
            : this(_ => execute())
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
        }
        
        public AquaCommand(Action<object> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public AquaCommand(Action execute, Func<bool> canExecute)
            : this(_ => execute(), _ => canExecute())
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            
            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }
        
        public AquaCommand(Action<object> execute, Func<object, bool> canExecute)
            : base(canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }
        
        private protected sealed override void ExecuteCore(object parameter)
            => Execute(parameter);
        
        public void Execute(object parameter = null)
        {
            if (!CanExecute(parameter))
                return;

            IsExecuting = true;

            (_execute ?? ExecuteInternal).Invoke(parameter);

            IsExecuting = false;
        }
        
        protected virtual void ExecuteInternal(object parameter) { }
    }
}