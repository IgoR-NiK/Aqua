namespace Aqua.Core.Services
{
    public class JsonConfigNameDefaultProvider : IJsonConfigNameProvider
    {
        public string GetConfigName<TConfig>() 
            where TConfig : class, IConfig, new()
        {
            var configName = typeof(TConfig).Name;
            return $"{configName}.json";
        }
    }
}