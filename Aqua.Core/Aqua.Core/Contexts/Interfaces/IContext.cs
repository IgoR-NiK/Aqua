using System.Collections.Generic;
using Aqua.Core.Ioc;

namespace Aqua.Core.Contexts
{
    public interface IContext : IResolvable
    {
        IContext ParentContext { get; }
        
        IEnumerable<IContext> ChildrenContexts { get; }
    }
}