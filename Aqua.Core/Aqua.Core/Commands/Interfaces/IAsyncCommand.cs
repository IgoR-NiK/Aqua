using System;
using System.Threading.Tasks;
using Aqua.Core.Utils;

namespace Aqua.Core.Commands
{
    public interface IAsyncCommand : IAquaCommandBase, IWithCanExecute, IWithFaultedHandler, IWithCancel, IWithIsCancelled, IWithTimeout
    {
        Task ExecuteAsync();

        Action<OperationCanceledException> CancelledHandler { get; set; }
        
        Func<OperationCanceledException, Task> CancelledHandlerAsync { get; set; }
        
        Func<Exception, Task> FaultedHandlerAsync { get; set; }
    }
}