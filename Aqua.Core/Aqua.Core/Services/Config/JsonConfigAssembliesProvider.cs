using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aqua.Core.Utils;

namespace Aqua.Core.Services
{
    public class JsonConfigAssembliesProvider : IJsonConfigAssembliesProvider
    {
        public IEnumerable<Assembly> GetAssemblies<TConfig>()
            where TConfig : class, IConfig, new()
            => new[] { typeof(TConfig).Assembly }.Union(typeof(TConfig).GetDependentAssemblies());
    }
}