using System;

namespace Aqua.Core.Commands
{
    public interface IAsyncCancelledCommand : IAsyncCommand
    {
        TimeSpan? Timeout { get; set; }

        void Cancel();
    }
}