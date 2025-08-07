using System;

namespace Assets.Code.Animation
{
    public interface IAnimator<T> where T : Enum
    {
        public void Play(T animation);
    }
}