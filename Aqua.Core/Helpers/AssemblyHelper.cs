using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Aqua.Core.Extensions;

namespace Aqua.Core.Helpers
{
    public static class AssemblyHelper
    {
        public static IEnumerable<Assembly> GetDependentAssemblies(this Type type)
        {
            return type.Assembly.GetDependentAssemblies();
        }
        
        public static IEnumerable<Assembly> GetDependentAssemblies(this Assembly assembly)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(it => assembly.FullName.IsOneOf(GetNamesOfAssembliesReferencedBy(it)));
        }

        public static IEnumerable<string> GetNamesOfAssembliesReferencedBy(Assembly assembly)
        {
            return assembly.GetReferencedAssemblies()
                .Select(assemblyName => assemblyName.FullName);
        }
    }
}