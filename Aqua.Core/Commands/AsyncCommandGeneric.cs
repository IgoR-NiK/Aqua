using System;
using System.Threading.Tasks;

namespace Aqua.Core.Commands
{
    public class AsyncCommand<T> : AsyncCommand
    {
        public AsyncCommand()
        {
            _execute = o => ExecuteInternal((T)o);
            _canExecute = o => IsValidParameter(o) && CanExecuteInternal((T)o);
        }
        
        public AsyncCommand(Func<T, Task> execute)
            : base(o => execute((T)o), IsValidParameter)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
        }

        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute)
            : base(o => execute((T)o), o => IsValidParameter(o) && canExecute((T)o))
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }

        protected sealed override Task ExecuteInternal(object parameter) 
            => base.ExecuteInternal(parameter);

        protected sealed override bool CanExecuteInternal(object parameter)
            => base.CanExecuteInternal(parameter);
        
        protected virtual Task ExecuteInternal(T parameter) => Task.CompletedTask;

        protected virtual bool CanExecuteInternal(T parameter) => true;
        
        private static bool IsValidParameter(object o)
        {
            var type = typeof(T);
            return o != null ? o is T : Nullable.GetUnderlyingType(type) != null || !type.IsValueType;
        }
    }
}