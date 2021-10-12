namespace Aqua.Core.Services
{
    public class DefaultConfigProvider : IConfigProvider
    {
        public bool IsSuitableFor<TConfig>() where TConfig : class, IConfig, new()
            => true;

        public TConfig Get<TConfig>() where TConfig : class, IConfig, new()
            => new TConfig();
    }
}