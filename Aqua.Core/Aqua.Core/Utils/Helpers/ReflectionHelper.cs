using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Aqua.Core.Utils
{
    public static class ReflectionHelper
    {
        public static void CallByReflectionFrom<T>(
            T obj, 
            string methodName, 
            Type[] typeArguments, 
            params object[] parameters)
        {
            var genericMethod = MakeGenericMethodFrom<T>(methodName, typeArguments);
            genericMethod.Invoke(obj, parameters);
        }
		
        public static async Task CallByReflectionFromAsync<T>(
            T obj,
            string methodName, 
            Type[] typeArguments, 
            params object[] parameters)
        {
            var genericMethod = MakeGenericMethodFrom<T>(methodName, typeArguments);
            await genericMethod.InvokeAsync(obj, parameters);
        }
        
        public static MethodInfo MakeGenericMethodFrom<T>(string methodName, params Type[] typeArguments)
        {
            var method = typeof(T).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            var genericMethod = method?.MakeGenericMethod(typeArguments);
            return genericMethod;
        }
    }
}