using Assets.Code.Tools;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Code
{
    public class SwordStrike : Ability
    {
        private const int MaxStrikeCount = 50;

        private readonly ParticleSystem _swingEffect;
        private readonly LayerMask _damageLayer;
        private readonly Collider[] _colliders;

        private float _damage;
        private float _radius;

        public SwordStrike(AbilityConfig config, Transform transform, int level = 1) : base(config, transform, level)
        {
            config.ThrowIfNull();

            _colliders = new Collider[MaxStrikeCount];
            _swingEffect = Object.Instantiate(config.Effect, transform.ThrowIfNull());
            _damageLayer = config.DamageLayer;

            AbilityStats stats = config.GetStats(level.ThrowIfZeroOrLess());

            _damage = stats.Damage;
            _radius = stats.Range;
            SetEffectShape();
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
        }

        protected override void UpdateStats(float damage, float range, float projectilesCount)
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
