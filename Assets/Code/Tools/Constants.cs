using Assets.Code;
using Assets.Code.AmplificationSystem;
using System;

namespace Assets.Scripts.Tools
{
    public static class Constants
    {
        public const int Zero = 0;
        public const int One = 1;
        public const int Two = 2;
        public const int Hundred = 100;
        public const int FullCircleDegrees = 360;

        public static float PercentToMultiplier(float value)
        {
            return One - value.ThrowIfNegative() / Hundred;
        }

        public static AbilityType[] AbilityTypes => (AbilityType[])Enum.GetValues(typeof(AbilityType));
        public static BuffType[] BuffTypes => (BuffType[])Enum.GetValues(typeof(BuffType));

        public static Enum[] GetEnums<T>() where T : Enum
        {
            return (Enum[])Enum.GetValues(typeof(T));
        }

        public static Enum[] GetEnums(Type enumType)
        {
            enumType.IsEnum.ThrowIfFalse();

            return (Enum[])Enum.GetValues(enumType);
        }
    }
}
