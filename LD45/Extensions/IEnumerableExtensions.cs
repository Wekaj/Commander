using System;
using System.Collections.Generic;
using System.Linq;

namespace LD45.Extensions {
    public static class IEnumerableExtensions {
        public static T Random<T>(this IEnumerable<T> enumerable, Random random) {
            IList<T> list = enumerable as IList<T> ?? enumerable.ToList();
            return list[random.Next(list.Count)];
        }

        public static T RandomOrDefault<T>(this IEnumerable<T> enumerable, Random random) {
            IList<T> list = enumerable as IList<T> ?? enumerable.ToList();

            if (list.Count > 0) {
                return list[random.Next(list.Count)];
            }
            else {
                return default;
            }
        }

        public static IEnumerable<T> RandomGroup<T>(this IEnumerable<T> enumerable, int groupSize, Random random) {
            IList<T> list = enumerable as IList<T> ?? enumerable.ToList();
            list.Shuffle(random);
            return list.Take(groupSize);
        }

        public static IEnumerable<T> Merge<T>(this IEnumerable<IEnumerable<T>> enumerable) {
            IEnumerable<T> result = null;
            foreach (IEnumerable<T> item in enumerable) {
                result = result?.Concat(item) ?? item;
            }
            return result;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable, Random random) {
            IList<T> list = enumerable as IList<T> ?? enumerable.ToList();
            list.Shuffle(random);
            return list;
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T item) {
            foreach (T enumItem in enumerable) {
                yield return enumItem;
            }

            yield return item;
        }

        public static IEnumerable<T> ConcatTo<T>(this IEnumerable<T> enumerable, T item) {
            yield return item;

            foreach (T enumItem in enumerable) {
                yield return enumItem;
            }
        }
    }
}