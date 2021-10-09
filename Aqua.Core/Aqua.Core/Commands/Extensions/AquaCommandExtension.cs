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
        
        public static TCommand WithCancelledHandler<TCommand>(this TCommand command, Action<OperationCanceledException> cancelledHandler)
            where TCommand : IAsyncCommand
        {
            command.CancelledHandler = cancelledHandler;
            return command;
        }

        public static TCommand WithCancelledHandlerAsync<TCommand>(this TCommand command, Func<OperationCanceledException, Task> cancelledHandlerAsync)
            where TCommand : IAsyncCommand
        {
            command.CancelledHandlerAsync = cancelledHandlerAsync;
            return command;
        }
        
        public static TCommand WithCancelledHandler<TCommand, TParam>(this TCommand command, Action<TParam, OperationCanceledException> cancelledHandler)
            where TCommand : IAsyncCommand<TParam>
        {
            command.CancelledHandler = cancelledHandler;
            return command;
        }

        public static TCommand WithCancelledHandlerAsync<TCommand, TParam>(this TCommand command, Func<TParam, OperationCanceledException, Task> cancelledHandlerAsync)
            where TCommand : IAsyncCommand<TParam>
        {
            command.CancelledHandlerAsync = cancelledHandlerAsync;
            return command;
        }
    }
}