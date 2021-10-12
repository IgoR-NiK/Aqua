using Aqua.Core.Ioc;

namespace Aqua.Core.Services
{
    public interface IJsonConfigNameProvider : IResolvable
    {
        string GetConfigName<TConfig>() 
            where TConfig : class, IConfig, new();
    }
}