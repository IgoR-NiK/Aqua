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
        {
            foreach (var item in items)
            {
                @this.Add(item);
            }
        }
    }
}