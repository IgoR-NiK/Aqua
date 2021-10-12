using System.Collections.Generic;
using System.Reflection;
using Aqua.Core.Ioc;

namespace Aqua.Core.Services
{
    public interface IJsonConfigAssembliesProvider : IResolvable
    {
        IEnumerable<Assembly> GetAssemblies<TConfig>() 
            where TConfig : class, IConfig, new();
    }
}