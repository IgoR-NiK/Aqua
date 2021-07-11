using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Aqua.Core.Utils
{
    public static class ObservableCollectionExtension
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
            => new ObservableCollection<T> { items };

        public static void Add<T>(this ObservableCollection<T> @this, IEnumerable<T> items)
            => @this.AddRange(items);

        public static void AddRange<T>(this ObservableCollection<T> @this, IEnumerable<T> items)
            => items.ForEach(@this.Add);
    }
}