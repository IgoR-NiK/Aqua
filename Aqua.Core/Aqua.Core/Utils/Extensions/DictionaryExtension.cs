using System;
using System.Collections.Generic;

namespace Aqua.Core.Utils
{
    public static class DictionaryExtension
    {
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
            => dictionary.TryGetValue(key, out var value) 
                ? value 
                : dictionary[key] = valueFactory(key);
    }
}