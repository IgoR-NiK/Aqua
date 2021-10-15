using Aqua.Core.Ioc;
using Aqua.Core.Services;
using DryIoc;

namespace Aqua.Core
{
    public sealed class AquaCoreIocModule : IIocModule
    {
        public void RegisterTypes(IRegistrator container)
        {
            // ConfigService
            container.Register(typeof(IJsonConfigAssembliesProvider<>), typeof(JsonConfigAssembliesProvider<>), Reuse.Singleton);
            container.Register(typeof(IJsonConfigNamespacesProvider<>), typeof(JsonConfigNamespacesProvider<>), Reuse.Singleton);
            container.Register(typeof(IJsonConfigNamesProvider<>), typeof(JsonConfigNamesProvider<>), Reuse.Singleton);
            container.Register(typeof(IConfigProvider<>), typeof(DefaultConfigProvider<>), Reuse.Singleton);
        }
    }
}