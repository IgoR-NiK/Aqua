using System;
using System.Threading.Tasks;
using Aqua.Core.Utils;

namespace Aqua.Core.Commands
{
    public interface IAsyncCommand : IAquaCommandBase, IWithCanExecute, IWithCancel, IWithTimeout
    {
        Task ExecuteAsync();

        Action<OperationCanceledException> Cancelled { get; set; }
        
        Func<OperationCanceledException, Task> CancelledAsync { get; set; }
    }
}