using System;

namespace Aqua.Core.Commands
{
    public interface IAsyncCancelledCommand : IAquaCommand
    {
        TimeSpan? Timeout { get; set; }

        void Cancel();
    }
}