using System;

namespace Aqua.Core.Utils
{
    public interface IWithFaultedHandler
    {
        Action<Exception> FaultedHandler { get; set; }
    }
}