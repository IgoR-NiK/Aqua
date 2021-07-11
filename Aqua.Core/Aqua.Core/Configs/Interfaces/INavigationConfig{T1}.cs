using Aqua.Core.Contexts;

namespace Aqua.Core.Configs
{
    public interface INavigationConfig<out TContext> : INavigationConfig, IConfig<TContext>
        where TContext : class, IContext
    {
    }
}