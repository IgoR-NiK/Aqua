using System;
using System.ComponentModel;
using Aqua.Core.Services;

namespace Aqua.XamarinForms.Core.Services
{
    public interface IViewModelWrapper<in TResult> : IViewModelWrapper
    {
        Action<TResult, CancelEventArgs> ViewClosing { get; }
			
        Action<TResult> ViewClosed { get; }
    }
}