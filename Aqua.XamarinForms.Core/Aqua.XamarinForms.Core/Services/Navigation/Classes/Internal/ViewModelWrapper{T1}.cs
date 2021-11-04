using System;
using System.Collections.Generic;
using System.ComponentModel;
using Aqua.Core.Mvvm;
using Aqua.Core.Services;

namespace Aqua.XamarinForms.Core.Services
{
    internal sealed class ViewModelWrapper<TResult> : IViewModelWrapper<TResult>
    {
        public ViewModelBase Parent { get; set; }

        public List<ViewModelBase> Children { get; set; } = new List<ViewModelBase>();
			
        public Action<TResult, CancelEventArgs> ViewClosing  { get; }

        public Action<TResult> ViewClosed { get; }

        public ViewModelWrapper(CallbackParam<TResult> callbackParam)
        {
            ViewClosing = callbackParam?.ViewClosing;
            ViewClosed = callbackParam?.ViewClosed;
        }
    }
}