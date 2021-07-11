using System.Collections.Generic;
using System.Windows.Controls;
using Aqua.Core.Contexts;
using Aqua.Core.Mvvm;

namespace Aqua.Wpf.Core.Mvvm
{
    public abstract class ViewBase : UserControl, IView
    {
        public IContext ParentContext { get; }
        
        public IEnumerable<IContext> ChildrenContexts { get; }
    }
}