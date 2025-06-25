using System;
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

        public static void ThrowIfNull<T>(this T argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException();
            }
        }

        public static void ThrowIfLessThan(this int value, int range)
        {
            if (value < range)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public static void ThrowIfZeroOrLess(this int value)
        {
            if (value <= Constants.Zero)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public static void ThrowIfZeroOrLess(this float value)
        {
            if (value < Constants.Zero || Mathf.Approximately(value, Constants.Zero))
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
