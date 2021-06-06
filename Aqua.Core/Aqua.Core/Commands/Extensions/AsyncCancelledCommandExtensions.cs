using System;

namespace Aqua.Core.Commands
{
    public static class AsyncCancelledCommandExtensions
    {
        public static TCommand WithTimeout<TCommand>(this TCommand command, TimeSpan timeout) 
            where TCommand : IAsyncCancelledCommand
        {
            command.Timeout = timeout;
            return command;
        }
        
        public static TCommand WithTimeout<TCommand>(this TCommand command, double timeInMilliseconds) 
            where TCommand : IAsyncCancelledCommand
            => command.WithTimeout(TimeSpan.FromMilliseconds(timeInMilliseconds));
    }
}