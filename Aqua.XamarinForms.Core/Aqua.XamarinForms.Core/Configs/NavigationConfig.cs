using System;
using Aqua.Core.Services;
using Aqua.Core.Utils;

namespace Aqua.XamarinForms.Core.Configs
{
    [JsonConfig]
    [CanBeCached]
    public class NavigationConfig
    {
        public Type StackType { get; set; } = typeof(NavigationStack);

        public bool WithAnimation { get; set; } = true;
    }
}