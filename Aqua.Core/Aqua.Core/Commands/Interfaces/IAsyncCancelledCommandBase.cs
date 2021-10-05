using System;

namespace Aqua.Core.Commands
{
    public interface IAsyncCancelledCommandBase : IAquaCommandBase
    {
        TimeSpan? Timeout { get; set; }

        void Cancel();
    }
}