using Aqua.Core.Contexts;

namespace Aqua.Core.Configs
{
    public interface INavigationConfig<out TContext> : IConfig<TContext>
        where TContext : class, IContext
    {     
        // Здесь будет еще IStack
        
        bool WithAnimation { get; set; }
    }
}