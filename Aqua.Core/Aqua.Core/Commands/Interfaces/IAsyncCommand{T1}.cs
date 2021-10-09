using System;
using System.Threading.Tasks;
using Aqua.Core.Utils;

namespace Aqua.Core.Commands
{
    public interface IAsyncCommand<TParam> : IAquaCommandBase, IWithCanExecute<TParam>, IWithCancel, IWithIsCancelled, IWithTimeout
    {
        Task ExecuteAsync(TParam parameter);

        Action<TParam, OperationCanceledException> CancelledHandler { get; set; }
        
        Func<TParam, OperationCanceledException, Task> CancelledHandlerAsync { get; set; }
    }
}