using Aqua.Core.Contexts;

namespace Aqua.Core.Configs
{
    public class NavigationAppConfig : INavigationConfig<IAppContext>
    {
        public IAppContext Context { get; }
        
        public bool WithAnimation { get; set; } = true;

        public NavigationAppConfig(IAppContext context)
        {
            Context = context;
        }
    }
}