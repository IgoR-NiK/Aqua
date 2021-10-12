using System.Collections.Generic;
using Aqua.Core.Ioc;

namespace Aqua.Core.Services
{
    public interface IJsonConfigNamesProvider : IResolvable
    {
        IEnumerable<string> GetConfigNames<TConfig>() 
            where TConfig : class, IConfig, new();
    }
}