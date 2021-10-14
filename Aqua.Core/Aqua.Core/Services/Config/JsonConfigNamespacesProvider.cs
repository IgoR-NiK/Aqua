using System.Collections.Generic;
using System.Reflection;

namespace Aqua.Core.Services
{
    public sealed class JsonConfigNamespacesProvider<TConfig> : IJsonConfigNamespacesProvider<TConfig>
        where TConfig : class, IConfig, new()
    {
        public IEnumerable<string> GetNamespaces(Assembly assembly)
        {
            yield return $"{assembly.GetName().Name}.Configs";
        }
    }
}