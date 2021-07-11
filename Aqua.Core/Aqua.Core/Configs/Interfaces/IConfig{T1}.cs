using Aqua.Core.Contexts;

namespace Aqua.Core.Configs
{
    public interface IConfig<out TContext>
        where TContext : class, IContext
    {
        TContext Context { get; }
    }
}