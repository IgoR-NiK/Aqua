using System;
using Aqua.Core.Services;
using Aqua.Core.Utils;

namespace Aqua.Core.Configs
{
    [JsonConfig]
    [CanBeCached]
    public class NavigationConfig : IConfig
    {
        public Type StackType { get; set; } = typeof(NavigationStack);
        
        public bool WithAnimation { get; set; } = true;
    }
}