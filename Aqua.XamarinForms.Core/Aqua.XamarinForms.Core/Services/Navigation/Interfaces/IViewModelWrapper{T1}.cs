using System;
using Aqua.Core.Services;

namespace Aqua.XamarinForms.Core.Services
{
    public interface IViewModelWrapper<in TResult> : IViewModelWrapper
    {
        Action<TResult, ViewClosingArgs> ViewClosing { get; }
			
        Action<TResult> ViewClosed { get; }
    }
}