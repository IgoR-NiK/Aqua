using System;
using Aqua.Core.Ioc;
using Aqua.Core.Utils;

namespace Aqua.Core.Services
{
    public class CacheConfigService<TConfig> : IDecorator<IConfigService<TConfig>>, IConfigService<TConfig>
        where TConfig : class, IConfig, new()
    {
        private TConfig _cached;
            
        public IConfigService<TConfig> Decoratee { get; }
        
        public CacheConfigService(IConfigService<TConfig> decoratee)
        {
            Decoratee = decoratee;
        }
        
        public TConfig Get()
        {
            return Attribute.IsDefined(typeof(TConfig), typeof(CanBeCachedAttribute))
                ? _cached ??= Decoratee.Get()
                : Decoratee.Get();
        }
    }
}