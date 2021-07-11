using System;
using Aqua.Core.Contexts;

namespace Aqua.Core.Utils
{
    public static class ContextExtension
    {
        public static IAppContext GetAppContext(this IContext context)
        {
            return context.FindContext<IAppContext>();
        }

        public static bool TryFindContext<TContext>(this IContext context, out TContext foundContext, Func<TContext, bool> predicate = null)
            where TContext : class, IContext
        {
            foundContext = context.FindContext(predicate);
            return foundContext != null;
        }

        public static TContext FindContext<TContext>(this IContext context, Func<TContext, bool> predicate = null)
            where TContext : class, IContext
        {
            var searchContext = context;
            
            while (!(searchContext is TContext typedContext && (predicate?.Invoke(typedContext) ?? true)))
            {
                searchContext = searchContext.ParentContext;
            }

            return searchContext as TContext;
        }
    }
}