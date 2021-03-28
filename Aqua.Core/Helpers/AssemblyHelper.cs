using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Aqua.Core.Extensions;

namespace Aqua.Core.Helpers
{
    public static class AssemblyHelper
    {
        public static IEnumerable<Assembly> GetDependentAssemblies(Assembly analyzedAssembly)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(it => analyzedAssembly.FullName.IsOneOf(GetNamesOfAssembliesReferencedBy(it)));
        }

        public static IEnumerable<string> GetNamesOfAssembliesReferencedBy(Assembly assembly)
        {
            return assembly.GetReferencedAssemblies()
                .Select(assemblyName => assemblyName.FullName);
        }
    }
}