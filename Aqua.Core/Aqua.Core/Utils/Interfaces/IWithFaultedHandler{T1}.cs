using System;

namespace Aqua.Core.Utils
{
    public interface IWithFaultedHandler<TParam>
    {
        Action<TParam, Exception> FaultedHandler { get; set; }
    }
}