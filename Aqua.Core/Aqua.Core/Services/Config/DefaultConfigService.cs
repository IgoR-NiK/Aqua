namespace Aqua.Core.Services
{
    public class DefaultConfigService<TConfig> : IConfigService<TConfig>
        where TConfig : class, IConfig, new()
    {
        public TConfig Get() => new TConfig();
    }
}