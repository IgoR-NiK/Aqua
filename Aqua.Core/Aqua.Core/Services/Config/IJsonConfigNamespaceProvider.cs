using System.Reflection;
using Aqua.Core.Ioc;

namespace Aqua.Core.Services
{
    public interface IJsonConfigNamespaceProvider : IResolvable
    {
        string GetNamespace(Assembly assembly);
    }
}