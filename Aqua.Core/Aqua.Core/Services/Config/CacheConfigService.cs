using System;
using System.Collections.Concurrent;
using Aqua.Core.Ioc;

namespace Aqua.Core.Services
{
    public class CacheConfigService : IDecorator<IConfigService>, IConfigService 
    {
        private ConcurrentDictionary<Type, object> Cache { get; } = new ConcurrentDictionary<Type, object>();
        
        public IConfigService Decoratee { get; }
        
        public CacheConfigService(IConfigService decoratee)
        {
            Decoratee = decoratee;
        }

        public TConfig Get<TConfig>() where TConfig : class, IConfig, new()
        {
            return CanBeCached<TConfig>()
                ? (TConfig)Cache.GetOrAdd(typeof(TConfig), _ => Decoratee.Get<TConfig>())
                : Decoratee.Get<TConfig>();
        }

        private static bool CanBeCached<TConfig>() where TConfig : class, IConfig, new()
            => Attribute.IsDefined(typeof(TConfig), typeof(JsonConfigAttribute));
    }
}