using System;
using Aqua.Core.Utils;
using DryIoc;

namespace Aqua.Core.Ioc
{
    public static class ResolverExtension
    {
        public static T Resolve<T, TParam>(this IResolver resolver, TParam param)
            where T : class, IWithInit<TParam>
        {
            var resolvable = resolver.Resolve<T>();
            resolvable.Init(param);

            return resolvable;
        }
		
        public static object Resolve<TParam>(this IResolver resolver, Type type, TParam param)
        {
            var resolvable = resolver.Resolve(type);

            if (resolvable is IWithInit<TParam> resolvableWithInit)
            {
                resolvableWithInit.Init(param);
            }

            return resolvable;
        }
    }
}