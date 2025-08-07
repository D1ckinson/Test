using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Tools
{
    public static class Extensions
    {
        private const int Zero = 0;
        private const int One = 1;

        public static bool IsNull<T>(this T argument)
        {
            return argument == null;
        }

        public static bool NotNull<T>(this T argument)
        {
            return argument != null;
        }

        public static TValue GetValueOrThrow<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.TryGetValue(key, out TValue value))
            {
                return value;
            }

            throw new KeyNotFoundException();
        }

        public static void ForEach<T>(this ICollection<T> collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action?.Invoke(item);
            }
        }

        public static string ToWrapText(this IEnumerable<string> strings)
        {
            return string.Join(Environment.NewLine, strings);
        }

        public static void PlayAsNew(this Animator animator, int hash, int layer = 0)
        {
            animator.Play(hash, layer, 0);
        }

        public static int LastIndex<T>(this ICollection<T> collection)
        {
            if (collection is IDictionary dictionary)
            {
                return dictionary.Values.Count - One;
            }

            return collection.Count - One;
        }

        public static bool Contains(this LayerMask mask, int layer)
        {
            return (mask.value & (One << layer)) != Zero;
        }

        public static void SetActive(this Behaviour component, bool isActive)
        {
            component.gameObject.SetActive(isActive);
        }

        public static bool IsFinished(this AnimatorStateInfo stateInfo)
        {
            return stateInfo.normalizedTime >= One || stateInfo.loop;
        }
    }
}
