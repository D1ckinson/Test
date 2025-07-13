using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Tools
{
    public static class ThrowHelper
    {
        public static void ThrowIfNotNormalize(this Vector3 vector)
        {
            if (Mathf.Approximately(vector.sqrMagnitude, Constants.One) == false)
            {
                throw new VectorNotNormalizedException();
            }
        }

        public static void ThrowIfCollectionNull(this IEnumerable collection)
        {
            collection.ThrowIfNull();

            foreach (var item in collection)
            {
                item.ThrowIfNull();
            }
        }

        public static void ThrowIfDefault<T>(this T argument) where T : struct
        {
            if (argument.Equals(default(T)))
            {
                throw new ArgumentException();
            }
        }

        public static void ThrowIfFalse(this bool isValid, Exception exception = null)
        {
            if (isValid == false)
            {
                throw exception ?? new MissingComponentException();//
            }
        }

        public static T ThrowIfNull<T>(this T argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException();
            }

            return argument;
        }

        public static int ThrowIfLessThan(this int value, int range)
        {
            if (value < range)
            {
                throw new ArgumentOutOfRangeException();
            }

            return value;
        }

        public static int ThrowIfZeroOrLess(this int value)
        {
            if (value <= Constants.Zero)
            {
                throw new ArgumentOutOfRangeException();
            }

            return value;
        }

        public static float ThrowIfMoreThan(this float value, float range)
        {
            if (value > range)
            {
                throw new ArgumentOutOfRangeException();
            }

            return value;
        }

        public static int ThrowIfMoreThan(this int value, int range)
        {
            if (value > range)
            {
                throw new ArgumentOutOfRangeException();
            }

            return value;
        }

        public static float ThrowIfZeroOrLess(this float value)
        {
            if (value < Constants.Zero || Mathf.Approximately(value, Constants.Zero))
            {
                throw new ArgumentOutOfRangeException();
            }

            return value;
        }

        public static float ThrowIfNegative(this float value)
        {
            if (value < Constants.Zero)
            {
                throw new ArgumentOutOfRangeException();
            }

            return value;
        }

        public static int ThrowIfNegative(this int value)
        {
            if (value < Constants.Zero)
            {
                throw new ArgumentOutOfRangeException();
            }

            return value;
        }
    }
}
