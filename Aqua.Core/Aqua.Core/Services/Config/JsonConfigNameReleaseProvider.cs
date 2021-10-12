namespace Aqua.Core.Services
{
    public class JsonConfigNameReleaseProvider : IJsonConfigNameProvider
    {
        public string GetConfigName<TConfig>() 
            where TConfig : class, IConfig, new()
        {
            var configName = typeof(TConfig).Name;
            return $"{configName}.Release.json"; 
        }
    }
}