using Aqua.Core.Ioc;

namespace Aqua.Core.Services
{
    public interface IConfigService<out TConfig> : IResolvable
        where TConfig : class, IConfig, new()
    {
        TConfig Get();
    }
}