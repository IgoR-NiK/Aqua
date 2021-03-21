using System;

namespace Aqua.XamarinForms.Services.Navigation.Classes
{
    public sealed class CallbackParam<TResult>
    {
        public Action<TResult, ViewClosingArgs> ViewClosing { get; set; }
        
        public Action<TResult> ViewClosed { get; set; }
    }
}