﻿using System.Collections.Generic;
using System.Reflection;
using Aqua.Core.Ioc;

namespace Aqua.Core.Services
{
    public interface IJsonConfigAssembliesProvider<TConfig> : IResolvable
        where TConfig : class, IConfig, new()
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}