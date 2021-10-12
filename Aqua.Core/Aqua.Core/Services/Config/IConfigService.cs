using Aqua.Core.Ioc;

namespace Aqua.Core.Services
{
    public interface IConfigService : IResolvable
    {
        TConfig Get<TConfig>() where TConfig : class, IConfig, new();
    }
}