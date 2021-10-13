using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Aqua.Core.Utils
{
    public static class AssemblyHelper
    {
        public static IEnumerable<Assembly> GetDependentAssemblies(this Type type)
            => type.Assembly.GetDependentAssemblies();
        
        public static IEnumerable<Assembly> GetDependentAssemblies(this Assembly assembly)
            => AppDomain.CurrentDomain.GetAssemblies()
                .Where(it => assembly.FullName.IsOneOf(GetNamesOfAssembliesReferencedBy(it)));

        public static IEnumerable<string> GetNamesOfAssembliesReferencedBy(Assembly assembly)
            => assembly.GetReferencedAssemblies()
                .Select(assemblyName => assemblyName.FullName);
    }
}