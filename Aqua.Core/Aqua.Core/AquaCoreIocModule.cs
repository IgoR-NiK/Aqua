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
            container.Register<IJsonConfigAssembliesProvider, JsonConfigAssembliesProvider>(Reuse.Singleton);
            container.Register<IJsonConfigNamespaceProvider, JsonConfigNamespaceProvider>(Reuse.Singleton);
            container.Register<IJsonConfigNamesProvider, JsonConfigNamesProvider>(Reuse.Singleton);
            container.Register<IConfigProvider, JsonConfigProvider>(Reuse.Singleton);
            container.Register<IConfigProvider, DefaultConfigProvider>(Reuse.Singleton);
            container.Register<IConfigService, ConfigService>(Reuse.Singleton);
            container.Register<IConfigService, CacheConfigService>(Reuse.Singleton, setup: Setup.Decorator);
        }
    }
}