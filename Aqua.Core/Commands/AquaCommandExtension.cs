using System;
using System.ComponentModel;
using System.Linq.Expressions;

using Aqua.Core.Helpers;

namespace Aqua.Core.Commands
{
    public static class AquaCommandExtension
    {
        public static TCommand ListenOn<TCommand, TObject, TProperty>(this TCommand command, TObject @object, Expression<Func<TObject, TProperty>> property) 
            where TCommand : IAquaCommand
            where TObject : INotifyPropertyChanged
        {
            var propertyName = ExpressionHelper.GetPropertyName(property);
            
            return command.ListenOn(@object, propertyName);
        }
        
        public static TCommand ListenOn<TCommand, TObject>(this TCommand command, TObject @object, string propertyName) 
            where TCommand : IAquaCommand
            where TObject : INotifyPropertyChanged
        {
            @object.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == propertyName)
                    command.RaiseCanExecuteChanged();
            };

            return command;
        }
    }
}