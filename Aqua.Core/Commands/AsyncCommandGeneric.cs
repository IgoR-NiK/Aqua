using System;
using System.Threading.Tasks;

namespace Aqua.Core.Commands
{
    public class AsyncCommand<T> : AsyncCommand
    {
        public AsyncCommand(Func<T, Task> execute)
            : base(o => execute((T)o))
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

        private static bool IsValidParameter(object o)
        {
            var type = typeof(T);
            return o is T || Nullable.GetUnderlyingType(type) != null || !type.IsValueType;
        }
    }
}