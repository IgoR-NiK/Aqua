using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Aqua.Core.Services
{
    public class ConfigService : IConfigService
    {
        private IEnumerable<IConfigProvider> ConfigProviders { get; }

        public ConfigService(IEnumerable<IConfigProvider> configProviders)
        {
            ConfigProviders = configProviders;
        }
        
        [return: NotNull]
        public TConfig Get<TConfig>() where TConfig : class, IConfig, new()
        {
            return ConfigProviders
                .Where(it => it.IsSuitableFor<TConfig>())
                .Select(it => it.Get<TConfig>())
                .First(it => it != null);
        }
    }
}