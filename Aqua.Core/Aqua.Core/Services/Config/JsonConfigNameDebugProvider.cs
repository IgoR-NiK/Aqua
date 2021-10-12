namespace Aqua.Core.Services
{
    public class JsonConfigNameDebugProvider : IJsonConfigNameProvider
    {
        public string GetConfigName<TConfig>() 
            where TConfig : class, IConfig, new()
        {
            var configName = typeof(TConfig).Name;
            return $"{configName}.Debug.json";
        }
    }
}