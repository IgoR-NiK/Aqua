using System.Reflection;

namespace Aqua.Core.Services
{
    public class JsonConfigNamespaceProvider : IJsonConfigNamespaceProvider
    {
        public string GetNamespace(Assembly assembly)
            => $"{assembly.GetName().Name}.Configs";
    }
}