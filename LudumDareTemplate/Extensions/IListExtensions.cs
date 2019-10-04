using System;
using System.Collections.Generic;

namespace LudumDareTemplate.Extensions {
    public static class IListExtensions {
        public static void Swap<T>(this IList<T> list, int index1, int index2) {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        public static void Shuffle<T>(this IList<T> list, Random random) {
            for (int i = 0; i < list.Count - 1; i++) {
                list.Swap(i, random.Next(i, list.Count));
            }
        }
    }
}