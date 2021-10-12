using Aqua.Core.Services;

namespace XamarinFormsApp.Configs
{
    [JsonConfig]
    public class TestConfig : IConfig
    {
        public string Name { get; set; }
        
        public int Age { get; set; }
    }
}