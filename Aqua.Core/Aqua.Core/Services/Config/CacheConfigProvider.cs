using System;
using Aqua.Core.Ioc;
using Aqua.Core.Utils;

namespace Aqua.Core.Services
{
    public sealed class CacheConfigProvider<TConfig> : IDecorator<IConfigProvider<TConfig>>, IConfigProvider<TConfig>
        where TConfig : class, IConfig, new()
    {
        private TConfig _cached;
            
        public IConfigProvider<TConfig> Decoratee { get; }
        
        public CacheConfigProvider(IConfigProvider<TConfig> decoratee)
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