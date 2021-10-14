using System.Collections.Generic;
using Aqua.Core.Mvvm;

namespace Aqua.XamarinForms.Core.Services
{
    public interface IViewModelWrapper
    {
        ViewModelBase Parent { get; }
			
        List<ViewModelBase> Children { get; }
    }
}