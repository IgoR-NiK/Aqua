using System;
using System.Collections.Generic;
using Aqua.Core.Ioc;
using Aqua.Core.Utils;

namespace Aqua.Core.Services
{
    public sealed class CacheConfigProvider<TConfig> : Decorator<IConfigProvider<TConfig>>, IConfigProvider<TConfig>
        where TConfig : class, IConfig, new()
    {
        private TConfig _cached;
        private Dictionary<Type, bool> CanBeCached { get; } = new Dictionary<Type, bool>();

        public CacheConfigProvider(IConfigProvider<TConfig> decoratee)
            : base(decoratee)
        {
        }
        
        public TConfig GetConfig()
            => CanBeCached.GetOrAdd(typeof(TConfig), type => Attribute.IsDefined(type, typeof(CanBeCachedAttribute)))
                ? _cached ??= Decoratee.GetConfig()
                : Decoratee.GetConfig();
    }
}