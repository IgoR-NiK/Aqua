using System.Collections.Generic;
using System.Reflection;
using Aqua.Core.Ioc;

namespace Aqua.Core.Services
{
    public interface IJsonConfigNamespacesProvider<TConfig> : IResolvable
        where TConfig : class, IConfig, new()
    {
        IEnumerable<string> GetNamespaces(Assembly assembly);
    }
}