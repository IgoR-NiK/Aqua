using System;

namespace Aqua.Core.Commands
{
    public static class AsyncCancelledCommandExtensions
    {
        public static TCommand WithTimeout<TCommand>(this TCommand command, TimeSpan timeout) 
            where TCommand : IAsyncCancelledCommandBase
        {
            command.Timeout = timeout;
            return command;
        }
        
        public static TCommand WithTimeout<TCommand>(this TCommand command, double timeInMilliseconds) 
            where TCommand : IAsyncCancelledCommandBase
            => command.WithTimeout(TimeSpan.FromMilliseconds(timeInMilliseconds));
    }
}