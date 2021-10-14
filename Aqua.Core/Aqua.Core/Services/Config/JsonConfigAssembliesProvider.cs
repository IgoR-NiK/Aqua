using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aqua.Core.Utils;

namespace Aqua.Core.Services
{
    public sealed class JsonConfigAssembliesProvider<TConfig> : IJsonConfigAssembliesProvider<TConfig>
        where TConfig : class, IConfig, new()
    {
        public IEnumerable<Assembly> GetAssemblies()
            => new[] { typeof(TConfig).Assembly }.Union(typeof(TConfig).GetDependentAssemblies());
    }
}