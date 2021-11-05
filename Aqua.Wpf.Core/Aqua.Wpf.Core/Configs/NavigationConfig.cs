using System;
using Aqua.Core.Services;
using Aqua.Core.Utils;

namespace Aqua.Wpf.Core.Configs
{
    [JsonConfig]
    [CanBeCached]
    public class NavigationConfig
    {
        public Type StackType { get; set; } = typeof(NavigationStack);

        public Type WindowType { get; set; } = typeof(AquaWindow);
    }
}