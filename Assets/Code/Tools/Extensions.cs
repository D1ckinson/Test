using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

        public static int ParseOrThrow(this string argument)
        {
            if (int.TryParse(argument, out var result))
            {
                return result;
            }

            throw new ArgumentException();
        }

        public static void SetText(this TMP_Text textWindow, string text)
        {
            textWindow.text = text;
        }

        public static TValue GetValueOrThrow<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.TryGetValue(key, out TValue value))
            {
                return value;
            }

            throw new KeyNotFoundException();
        }

        public static T GetComponentOrThrow<T>(this Component component)
        {
            if (component.TryGetComponent(out T value))
            {
                return value;
            }

            throw new MissingComponentException();
        }

        public static T GetComponentInChildrenOrThrow<T>(this Component component)
        {
            return component.GetComponentInChildren<T>() ?? throw new MissingComponentException();
        }

        public static void ForEach<T>(this ICollection<T> collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action?.Invoke(item);
            }
        }

        public static void ForEachValues<TKey, TValue>(this Dictionary<TKey, TValue> pairs, Action<TValue> action)
        {
            foreach (TValue item in pairs.Values)
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

        public static void SetActive(this Component component, bool isActive)
        {
            component.gameObject.SetActive(isActive);
        }

        public static bool IsFinished(this AnimatorStateInfo stateInfo)
        {
            return stateInfo.normalizedTime >= One || stateInfo.loop;
        }

        public static bool IsActive(this Component component)
        {
            return component.gameObject.activeSelf;
        }

        public static void Subscribe(this Button button, UnityAction action)
        {
            button.onClick.AddListener(action);
        }

        public static void UnsubscribeAll(this Button button)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
