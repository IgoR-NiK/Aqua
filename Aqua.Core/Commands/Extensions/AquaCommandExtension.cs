using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

using Aqua.Core.Utils;

namespace Aqua.Core.Commands
{
    public static class AquaCommandExtension
    {
        public static TCommand ListenOn<TCommand, TObject>(this TCommand command, TObject @object, params Expression<Func<TObject, object>>[] properties) 
            where TCommand : IAquaCommand
            where TObject : INotifyPropertyChanged
        {
            var propertyNames = properties.Select(ExpressionHelper.GetPropertyName).ToArray();
            
            return command.ListenOn(@object, propertyNames);
        }
        
        public static TCommand ListenOn<TCommand, TObject>(this TCommand command, TObject @object, params string[] propertyNames) 
            where TCommand : IAquaCommand
            where TObject : INotifyPropertyChanged
        {
            @object.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.IsOneOf(propertyNames))
                    command.RaiseCanExecuteChanged();
            };

            return command;
        }
    }
}