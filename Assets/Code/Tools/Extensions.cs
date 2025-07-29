using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Tools
{
    public static class Extensions
    {
        public static bool IsNull<T>(this T argument)
        {
            return argument == null;
        }

        public static TValue TryGetValueOrThrow<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.TryGetValue(key, out TValue value))
            {
                return value;
            }

            throw new KeyNotFoundException();
        }

        public static string ToTextWithNewLines(this IEnumerable<string> strings)
        {
            return string.Join(Environment.NewLine, strings);
        }

        public static int GetLastIndex<T>(this ICollection<T> collection)
        {
            if (collection is IDictionary dictionary)
            {
                return dictionary.Values.Count - 1;
            }

            return collection.Count - 1;
        }

        public static bool IsPositive(this float value)
        {
            return value > 0;
        }

        public static bool Contains(this LayerMask mask, int layer)
        {
            return (mask.value & (1 << layer)) != 0;
        }

        public static void SetActive(this Behaviour component, bool isActive)
        {
            component.gameObject.SetActive(isActive);
        }
    }
}
