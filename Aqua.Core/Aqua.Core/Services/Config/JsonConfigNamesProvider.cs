using System.Collections.Generic;

namespace Aqua.Core.Services
{
    public sealed class JsonConfigNamesProvider<TConfig> : IJsonConfigNamesProvider<TConfig>
        where TConfig : class, IConfig, new()
    {
        public IEnumerable<string> GetConfigNames()
        {
            var configName = typeof(TConfig).Name;
            
#if DEBUG
            yield return $"{configName}.Debug.json";
#else
            yield return $"{configName}.Release.json";
#endif
            yield return $"{configName}.json";
        }
    }
}