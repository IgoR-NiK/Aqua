using System.Collections.Generic;

namespace Aqua.Core.Contexts
{
    public interface IContext
    {
        IContext ParentContext { get; }
        
        IEnumerable<IContext> ChildrenContexts { get; }
    }
}