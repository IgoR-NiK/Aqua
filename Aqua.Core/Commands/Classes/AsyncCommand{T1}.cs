using System;
using System.Threading.Tasks;

namespace Aqua.Core.Commands
{
    public class AsyncCommand<T> : AsyncCommand
    {
        public AsyncCommand() { }
        
        public AsyncCommand(Func<T, Task> execute)
            : base(o => execute((T)o), IsValidParameter<T>)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
        }

        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute)
            : base(o => execute((T)o), o => IsValidParameter<T>(o) && canExecute((T)o))
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }

        public Task ExecuteAsync(T parameter)
            => base.ExecuteAsync(parameter);
        
        public bool CanExecute(T parameter)
            => base.CanExecute(parameter);

        protected sealed override Task ExecuteInternal(object parameter) 
            => ExecuteInternal((T)parameter);

        protected sealed override bool CanExecuteInternal(object parameter)
            => IsValidParameter<T>(parameter) && CanExecuteInternal((T)parameter);
        
        protected virtual Task ExecuteInternal(T parameter) => Task.CompletedTask;

        protected virtual bool CanExecuteInternal(T parameter) => true;
    }
}