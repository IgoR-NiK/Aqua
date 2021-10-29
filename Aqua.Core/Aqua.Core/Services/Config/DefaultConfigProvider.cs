namespace Aqua.Core.Services
{
    public sealed class DefaultConfigProvider<TConfig> : IConfigProvider<TConfig>
        where TConfig : class, IConfig, new()
    {
        public TConfig GetConfig() => new TConfig();
    }
}