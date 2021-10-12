using Aqua.Core.Ioc;
using Aqua.Core.Services;
using DryIoc;

namespace Aqua.Core
{
    public class AquaCoreIocModule : IIocModule
    {
        public void RegisterTypes(IRegistrator container)
        {
            // Config
#if DEBUG
            container.Register<IJsonConfigNameProvider, JsonConfigNameDebugProvider>(Reuse.Singleton);
#else
            container.Register<IJsonConfigNameProvider, JsonConfigNameReleaseProvider>(Reuse.Singleton);
#endif
            container.Register<IJsonConfigNameProvider, JsonConfigNameDefaultProvider>(Reuse.Singleton);
            container.Register<IConfigProvider, JsonConfigProvider>(Reuse.Singleton);
            container.Register<IConfigProvider, DefaultConfigProvider>(Reuse.Singleton);
            container.Register<IConfigService, ConfigService>(Reuse.Singleton);
            container.Register<IConfigService, CacheConfigService>(Reuse.Singleton, setup: Setup.Decorator);
        }
    }
}