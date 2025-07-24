namespace Assets.Scripts.Tools
{
    public static class Constants
    {
        public const int Zero = 0;
        public const int One = 1;
        public const int Two = 2;
        public const int FullCircleDegrees = 360;
        public const int Hundred = 100;

        public static float PercentToMultiplier(float value)
        {
            return One - value.ThrowIfNegative() / Hundred;
        }
    }
}
