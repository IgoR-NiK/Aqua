using System;
using Aqua.Core.Ioc;
using Aqua.Core.Utils;

namespace Aqua.Core.Services
{
    public sealed class CacheConfigProvider<TConfig> : Decorator<IConfigProvider<TConfig>>, IConfigProvider<TConfig>
        where TConfig : class, IConfig, new()
    {
        private TConfig _cached;
        
        public CacheConfigProvider(IConfigProvider<TConfig> decoratee)
            : base(decoratee)
        {
        }
        
        public TConfig GetConfig()
        {
            return Attribute.IsDefined(typeof(TConfig), typeof(CanBeCachedAttribute))
                ? _cached ??= Decoratee.GetConfig()
                : Decoratee.GetConfig();
        }
    }
}