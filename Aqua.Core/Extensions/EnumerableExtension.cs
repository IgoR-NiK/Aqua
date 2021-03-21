using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqua.Core.Extensions
{
    public static class EnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action?.Invoke(item);
            }
        }

        public static bool IsOneOf<T>(this T value, IEnumerable<T> source)
        {
            return source.Contains(value);
        }
    }
}