using Aqua.Core.Ioc;
using Aqua.Core.Services;
using DryIoc;

namespace Aqua.Core
{
    public sealed class AquaCoreIocModule : IIocModule
    {
        public void RegisterTypes(IRegistrator registrator)
        {
            // ConfigProvider
            registrator.Register(typeof(IJsonConfigAssembliesProvider<>), typeof(JsonConfigAssembliesProvider<>), Reuse.Singleton);
            registrator.Register(typeof(IJsonConfigNamespacesProvider<>), typeof(JsonConfigNamespacesProvider<>), Reuse.Singleton);
            registrator.Register(typeof(IJsonConfigNamesProvider<>), typeof(JsonConfigNamesProvider<>), Reuse.Singleton);
            registrator.Register(typeof(IConfigProvider<>), typeof(DefaultConfigProvider<>), Reuse.Singleton);
        }
    }
}