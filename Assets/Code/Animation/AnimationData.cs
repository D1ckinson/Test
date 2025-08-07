using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Code.Animation
{
    public readonly struct AnimationData
    {
        public readonly int Hash;
        public readonly int Priority;

        private AnimationData(int hash, int priority)
        {
            Hash = hash;
            Priority = priority.ThrowIfNegative();
        }

        public static AnimationData Create(Enum name)
        {
            int hash = Animator.StringToHash(name.ToString());
            int priority = Convert.ToInt32(name);

            return new(hash, priority);
        }
    }
}