namespace Aqua.Core.Configs
{
    public interface INavigationConfig : IConfig
    {
        // Здесь будет еще IStack
        
        bool WithAnimation { get; set; }
    }
}