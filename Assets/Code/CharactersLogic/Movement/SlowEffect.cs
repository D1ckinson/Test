using Assets.Scripts.Tools;
using System;

namespace Assets.Scripts.Movement
{
    public readonly struct SlowEffect
    {
        public readonly float Duration;
        public readonly float Multiplier;
        public readonly Type Source;

        public SlowEffect(float duration, float multiplier, Type source)
        {
            Duration = duration.ThrowIfZeroOrLess().ThrowIfMoreThan(Constants.One);
            Multiplier = multiplier.ThrowIfZeroOrLess();
            Source = source.ThrowIfNull();
        }
    }
}
