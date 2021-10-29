using System;
using System.IO;
using Aqua.Core.Ioc;
using Newtonsoft.Json;

namespace Aqua.Core.Services
{
    public sealed class JsonConfigProvider<TConfig> : Decorator<IConfigProvider<TConfig>>, IConfigProvider<TConfig>
        where TConfig : class, IConfig, new()
    {
        private IJsonConfigAssembliesProvider<TConfig> JsonConfigAssembliesProvider { get; }
        
        private IJsonConfigNamespacesProvider<TConfig> JsonConfigNamespacesProvider { get; }
        
        private IJsonConfigNamesProvider<TConfig> JsonConfigNamesProvider { get; }

        public JsonConfigProvider(
            IConfigProvider<TConfig> decoratee,
            IJsonConfigAssembliesProvider<TConfig> jsonConfigAssembliesProvider,
            IJsonConfigNamespacesProvider<TConfig> jsonConfigNamespacesProvider,
            IJsonConfigNamesProvider<TConfig> jsonConfigNamesProvider) 
            : base(decoratee)
        {
            JsonConfigAssembliesProvider = jsonConfigAssembliesProvider;
            JsonConfigNamespacesProvider = jsonConfigNamespacesProvider;
            JsonConfigNamesProvider = jsonConfigNamesProvider;
        }
        
        public TConfig GetConfig()
        {
            if (!Attribute.IsDefined(typeof(TConfig),typeof(JsonConfigAttribute)))
            {
                return Decoratee.GetConfig();
            }

            foreach (var assembly in JsonConfigAssembliesProvider.GetAssemblies())
            {
                foreach (var @namespace in JsonConfigNamespacesProvider.GetNamespaces(assembly))
                {
                    foreach (var configName in JsonConfigNamesProvider.GetConfigNames())
                    {
                        var stream = assembly.GetManifestResourceStream($"{@namespace}.{configName}");
                        if (stream == null)
                            continue;
                
                        using (var reader = new StreamReader(stream))
                        {
                            var json = reader.ReadToEnd();
                            var config = JsonConvert.DeserializeObject<TConfig>(json);
                        
                            if (config != null)
                                return config;
                        }
                    }
                }
            }
            
            return Decoratee.GetConfig();
        }
    }
}