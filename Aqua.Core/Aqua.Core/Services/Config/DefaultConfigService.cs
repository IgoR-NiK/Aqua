namespace Aqua.Core.Services
{
    public sealed class DefaultConfigService<TConfig> : IConfigService<TConfig>
        where TConfig : class, IConfig, new()
    {
        public TConfig Get() => new TConfig();
    }
}