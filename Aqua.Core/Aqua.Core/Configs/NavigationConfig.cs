using System;
using Aqua.Core.Services;

namespace Aqua.Core.Configs
{
    [JsonConfig]
    public class NavigationConfig : IConfig
    {
        public Type StackType { get; set; }
        
        public bool WithAnimation { get; set; } = true;
    }
}