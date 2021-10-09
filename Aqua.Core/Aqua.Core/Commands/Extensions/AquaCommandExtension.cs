using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Aqua.Core.Utils;

namespace Aqua.Core.Commands
{
    public static class AquaCommandExtension
    {
        public static TCommand ListenOn<TCommand, TObject>(this TCommand command, TObject @object, params Expression<Func<TObject, object>>[] properties) 
            where TCommand : IAquaCommandBase
            where TObject : INotifyPropertyChanged
        {
            var propertyNames = properties.Select(ExpressionHelper.GetPropertyName).ToArray();
            
            return command.ListenOn(@object, propertyNames);
        }
        
        public static TCommand ListenOn<TCommand, TObject>(this TCommand command, TObject @object, params string[] propertyNames) 
            where TCommand : IAquaCommandBase
            where TObject : INotifyPropertyChanged
        {
            @object.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.IsOneOf(propertyNames))
                    command.RaiseCanExecuteChanged();
            };

            return command;
        }
        
        public static TCommand WithCancelled<TCommand>(this TCommand command, Action<OperationCanceledException> cancelled)
            where TCommand : IAsyncCommand
        {
            command.Cancelled = cancelled;
            return command;
        }

        public static TCommand WithCancelledAsync<TCommand>(this TCommand command, Func<OperationCanceledException, Task> cancelledAsync)
            where TCommand : IAsyncCommand
        {
            command.CancelledAsync = cancelledAsync;
            return command;
        }
        
        public static TCommand WithCancelled<TCommand, TParam>(this TCommand command, Action<TParam, OperationCanceledException> cancelled)
            where TCommand : IAsyncCommand<TParam>
        {
            command.Cancelled = cancelled;
            return command;
        }

        public static TCommand WithCancelledAsync<TCommand, TParam>(this TCommand command, Func<TParam, OperationCanceledException, Task> cancelledAsync)
            where TCommand : IAsyncCommand<TParam>
        {
            command.CancelledAsync = cancelledAsync;
            return command;
        }
    }
}