using System;
using System.Collections.Generic;

namespace Bearded.Photones.Utilities {
    static class LinqExtensions {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var item in source) {
                action(item);
            }
        }

        public static TValue ValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) {
            dict.TryGetValue(key, out TValue value);
            return value;
        }
    }
}