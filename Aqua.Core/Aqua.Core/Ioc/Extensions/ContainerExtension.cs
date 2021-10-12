using System.Collections.Generic;
using Aqua.Core.Utils;
using DryIoc;

namespace Aqua.Core.Ioc
{
    public static class ContainerExtension
    {
        internal static void RegisterModules<TModule>(this IContainer container, IEnumerable<TModule> modules) 
            where TModule : IIocModule
            => modules.ForEach(it => it.RegisterTypes(container));
    }
}