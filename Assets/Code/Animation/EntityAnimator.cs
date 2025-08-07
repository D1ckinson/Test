using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Animation
{
    public sealed class EntityAnimator<T> : IAnimator<T> where T : Enum
    {
        private const float TransitionDuration = 0.1f;

        private readonly Animator _animator;
        private readonly Dictionary<T, AnimationData> _animationData;

        private AnimationData _currentAnimationInfo;

        public EntityAnimator(Animator animator)
        {
            _animator = animator.ThrowIfNull();
            _animationData = CreateAnimationData();
        }

        public void Play(T animation)
        {
            AnimationData animationInfo = _animationData.GetValueOrThrow(animation.ThrowIfNull());

            if (animationInfo.Hash == _currentAnimationInfo.Hash)
            {
                return;
            }

            if (animationInfo.Priority > _currentAnimationInfo.Priority || IsAnimationFinished())
            {
                _animator.CrossFade(animationInfo.Hash, TransitionDuration);
                _currentAnimationInfo = animationInfo;
            }
        }

        private bool IsAnimationFinished()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(Constants.Zero);
            bool isCurrentState = stateInfo.shortNameHash == _currentAnimationInfo.Hash;

            return isCurrentState && stateInfo.IsFinished();
        }

        private Dictionary<T, AnimationData> CreateAnimationData()
        {
            IEnumerable<T> animationsNames = Constants.GetEnums<T>();
            Dictionary<T, AnimationData> animationHashes = new(animationsNames.Count());

            foreach (T name in animationsNames)
            {
                AnimationData animationInfo = AnimationData.Create(name);
                _animator.ThrowIfAnimationMissing(animationInfo.Hash);

                animationHashes.Add(name, animationInfo);
            }

            return animationHashes;
        }
    }
}
