using System.Collections.Generic;

namespace Aqua.Core.Services
{
    public class JsonConfigNamesProvider : IJsonConfigNamesProvider
    {
        public IEnumerable<string> GetConfigNames<TConfig>() 
            where TConfig : class, IConfig, new()
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