using System;
using System.IO;
using Newtonsoft.Json;

namespace Aqua.Core.Services
{
    public class JsonConfigProvider : IConfigProvider
    {
        private IJsonConfigAssembliesProvider JsonConfigAssembliesProvider { get; }
        private IJsonConfigNamesProvider JsonConfigNamesProvider { get; }
        private IJsonConfigNamespaceProvider JsonConfigNamespaceProvider { get; }

        public JsonConfigProvider(
            IJsonConfigAssembliesProvider jsonConfigAssembliesProvider,
            IJsonConfigNamesProvider jsonConfigNamesProvider,
            IJsonConfigNamespaceProvider jsonConfigNamespaceProvider)
        {
            JsonConfigAssembliesProvider = jsonConfigAssembliesProvider;
            JsonConfigNamesProvider = jsonConfigNamesProvider;
            JsonConfigNamespaceProvider = jsonConfigNamespaceProvider;
        }

        public bool IsSuitableFor<TConfig>() where TConfig : class, IConfig, new()
            => Attribute.IsDefined(typeof(TConfig), typeof(JsonConfigAttribute));
        
        public TConfig Get<TConfig>() where TConfig : class, IConfig, new()
        {
            foreach (var assembly in JsonConfigAssembliesProvider.GetAssemblies<TConfig>())
            {
                foreach (var configName in JsonConfigNamesProvider.GetConfigNames<TConfig>())
                {
                    var @namespace = JsonConfigNamespaceProvider.GetNamespace(assembly);
                    
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

            return null;
        }
    }
}