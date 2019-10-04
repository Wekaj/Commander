using System.Collections.Generic;

namespace LudumDareTemplate.Extensions {
    public static class ICollectionExtensions {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items) {
            foreach (T item in items) {
                collection.Add(item);
            }
        }

        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items) {
            foreach (T item in items) {
                collection.Remove(item);
            }
        }
    }
}
