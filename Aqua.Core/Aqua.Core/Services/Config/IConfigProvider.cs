using Aqua.Core.Ioc;

namespace Aqua.Core.Services
{
    public interface IConfigProvider : IResolvable
    {
        bool IsSuitableFor<TConfig>() where TConfig : class, IConfig, new();
        
        TConfig Get<TConfig>() where TConfig : class, IConfig, new();
    }
}