using System;

namespace Aqua.Core.Commands
{
    public class AquaCommand<T> : AquaCommand
    {
        public AquaCommand() { }
        
        public AquaCommand(Action<T> execute)
            : base(o => execute((T)o), IsValidParameter<T>)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
        }

        public AquaCommand(Action<T> execute, Func<T, bool> canExecute)
            : base(o => execute((T)o), o => IsValidParameter<T>(o) && canExecute((T)o))
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }

        public void Execute(T parameter)
            => base.Execute(parameter);
        
        public bool CanExecute(T parameter)
            => base.CanExecute(parameter);

        protected sealed override void ExecuteInternal(object parameter) 
            => ExecuteInternal((T)parameter);

        protected sealed override bool CanExecuteInternal(object parameter)
            => IsValidParameter<T>(parameter) && CanExecuteInternal((T)parameter);
        
        protected virtual void ExecuteInternal(T parameter) { }

        protected virtual bool CanExecuteInternal(T parameter) => true;
    }
}