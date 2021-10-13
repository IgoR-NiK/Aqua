using System.Collections.Generic;
using Aqua.Core.Ioc;

namespace Aqua.Core.Services
{
    public interface IJsonConfigNamesProvider<TConfig> : IResolvable
        where TConfig : class, IConfig, new()
    {
        IEnumerable<string> GetConfigNames();
    }
}