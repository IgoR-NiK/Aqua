using System;
using System.Threading.Tasks;
using Aqua.Core.Utils;

namespace Aqua.Core.Commands
{
    public interface IAsyncCommand<TParam> : IAquaCommandBase, IWithCanExecute<TParam>, IWithCancel, IWithTimeout
    {
        Task ExecuteAsync(TParam parameter);

        Action<TParam, OperationCanceledException> Cancelled { get; set; }
        
        Func<TParam, OperationCanceledException, Task> CancelledAsync { get; set; }
    }
}