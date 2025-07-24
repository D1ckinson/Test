using Assets.Scripts.Tools;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Code.Tools
{
    public static class Extensions
    {
        public static TValue GetValueOrThrow<TKey, TValue>(this Dictionary<TKey, TValue> keyValuePairs, TKey key)
        {
            keyValuePairs.ThrowIfCollectionNullOrEmpty().TryGetValue(key.ThrowIfNull(), out TValue value).ThrowIfFalse(new KeyNotFoundException());

            return value;
        }

        public static bool IsEmpty(this ICollection collection)
        {
            return collection.Count == Constants.Zero;
        }

        public static int GetLastIndex<T>(this ICollection<T> collection)
        {
            return collection.Count - Constants.One;
        }
    }
}
