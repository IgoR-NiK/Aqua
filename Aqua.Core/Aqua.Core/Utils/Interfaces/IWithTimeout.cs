using System;

namespace Aqua.Core.Utils
{
    public interface IWithTimeout
    {
        TimeSpan? Timeout { get; set; }
    }
}