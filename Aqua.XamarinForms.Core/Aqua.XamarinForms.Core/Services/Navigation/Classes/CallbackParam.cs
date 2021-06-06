using System;

namespace Aqua.XamarinForms.Core.Services.Navigation
{
    public sealed class CallbackParam<TResult>
    {
        public Action<TResult, ViewClosingArgs> ViewClosing { get; set; }
        
        public Action<TResult> ViewClosed { get; set; }
    }
}