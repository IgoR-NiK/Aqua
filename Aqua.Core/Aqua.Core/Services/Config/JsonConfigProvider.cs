using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Aqua.Core.Services
{
    public class JsonConfigProvider : IConfigProvider
    {
        private IEnumerable<IJsonConfigNameProvider> JsonConfigNameProviders { get; }

        public JsonConfigProvider(IEnumerable<IJsonConfigNameProvider> jsonConfigNameProviders)
        {
            JsonConfigNameProviders = jsonConfigNameProviders;
        }

        public bool IsSuitableFor<TConfig>() where TConfig : class, IConfig, new()
            => Attribute.IsDefined(typeof(TConfig), typeof(JsonConfigAttribute));
        
        public TConfig Get<TConfig>() where TConfig : class, IConfig, new()
        {
            var jsonConfigAttribute = typeof(TConfig)
                .GetCustomAttributes(typeof(JsonConfigAttribute))
                .Single() as JsonConfigAttribute;

            var @namespace = jsonConfigAttribute?.Namespace ?? typeof(TConfig).Namespace;

            var assembly = typeof(TConfig).Assembly;
            foreach (var jsonConfigNameProvider in JsonConfigNameProviders)
            {
                var configName = jsonConfigNameProvider.GetConfigName<TConfig>();
                
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

            return null;
        }
    }
}