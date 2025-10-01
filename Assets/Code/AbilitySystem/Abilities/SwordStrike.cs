using Assets.Code.CharactersLogic;
using Assets.Code.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.AbilitySystem.Abilities
{
    public class SwordStrike : Ability
    {
        private const int MaxStrikeCount = 50;

        private readonly ParticleSystem _swingEffect;
        private readonly LayerMask _damageLayer;
        private readonly Collider[] _colliders;
        private readonly Animator _animator;

        private float _damage;
        private float _radius;

        public SwordStrike(AbilityConfig config, Transform transform, Animator animator, Dictionary<AbilityType, int> abilityUnlockLevel, int level = 1) : base(config, transform, abilityUnlockLevel, level)
        {
            _colliders = new Collider[MaxStrikeCount];
            _swingEffect = config.Effect.Instantiate(transform.ThrowIfNull());
            _damageLayer = config.DamageLayer;

            AbilityStats stats = config.ThrowIfNull().GetStats(level.ThrowIfZeroOrLess());

            _damage = stats.Damage;
            _radius = stats.Range;
            SetEffectShape();
            _animator = animator;
        }

        public override void Dispose()
        {
            _swingEffect.DestroyGameObject();
        }

        protected sealed override void Apply()
        {
            int count = Physics.OverlapSphereNonAlloc(GetPosition(), _radius, _colliders, _damageLayer);

            for (int i = Constants.Zero; i < count; i++)
            {
                Collider collider = _colliders[i];

                if (collider.TryGetComponent(out Health health) == false)
                {
                    continue;
                }

                health.TakeDamage(_damage);
            }

            _swingEffect.Play();
            _animator.SetTrigger(AnimationParameters.IsAttacking);
        }

        protected override void UpdateStats(float damage, float range, int projectilesCount, bool isPiercing, int healthPercent)
        {
            _damage = damage.ThrowIfNegative();
            _radius = range.ThrowIfNegative();
            SetEffectShape();
        }

        private void SetEffectShape()
        {
            ParticleSystem.ShapeModule shapeModule = _swingEffect.shape;
            shapeModule.radius = _radius;
        }
    }
}
