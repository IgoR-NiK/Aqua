using System;
using Aqua.Core.Services;

namespace Aqua.Core.Configs
{
    public interface INavigationConfig : IConfig
    {
        Type StackType { get; set; }
    }
}