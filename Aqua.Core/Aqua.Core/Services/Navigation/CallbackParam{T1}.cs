using System;

namespace Aqua.Core.Services
{
    public class CallbackParam<TResult>
    {
        public Action<TResult, ViewClosingArgs> ViewClosing { get; set; }
        
        public Action<TResult> ViewClosed { get; set; }
    }
}