using Aqua.Core.Contexts;

namespace Aqua.Core.Configs
{
    public interface IConfig<out TContext> : IConfig
        where TContext : class, IContext
    {
        TContext Context { get; }
    }
}