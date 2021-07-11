using System.Collections.Generic;
using Aqua.Core.Contexts;
using Aqua.Core.Mvvm;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Mvvm
{
    public abstract class ViewBase : Page, IView
    {
        public IContext ParentContext { get; }
        
        public IEnumerable<IContext> ChildrenContexts { get; }
    }
}