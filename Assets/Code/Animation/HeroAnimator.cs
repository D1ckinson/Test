﻿using Assets.Code.AbilitySystem.Abilities;
using Assets.Code.Tools;
using Assets.Scripts;
using Assets.Scripts.Movement;
using UnityEngine;

namespace Assets.Code.Animation
{
    public class HeroAnimator
    {
        private readonly IAnimator _animator;
        private readonly ITellDirection _tellDirection;
        private readonly SwordStrike _swordStrike;

        public HeroAnimator(Animator animator, Health health, ITellDirection tellDirection, SwordStrike swordStrike)
        {
            _animator = new EntityAnimator<HeroAnimation>(animator.ThrowIfNull());

            _tellDirection = tellDirection.ThrowIfNull();
            _swordStrike = swordStrike.ThrowIfNull();
            //_swordStrike.Applied+=_a
        }

        private void AnimateAttack()
        {

        }
    }
}
