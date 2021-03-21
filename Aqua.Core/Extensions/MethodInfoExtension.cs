using System.Reflection;
using System.Threading.Tasks;

namespace Aqua.Core.Extensions
{
    public static class MethodInfoExtension
    {
        public static async Task InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
        {
            var task = (Task)@this.Invoke(obj, parameters);
            await task;
        }
    }
}