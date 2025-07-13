using Assets.Scripts;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Code
{
    public class NewSwordStrike : Ability
    {
        private readonly NewAbilityConfig _config;
        private readonly ParticleSystem _swingEffect;

        private AbilityStats _stats;
        private int _level;
        private readonly Collider[] _colliders;

        public NewSwordStrike(NewAbilityConfig config, Transform transform, int level = Constants.Zero) : base(config.Type, transform)
        {
            _config = config.ThrowIfNull();
            _level = level.ThrowIfNegative();
            _stats = _config.GetStats(_level);
            _swingEffect = Object.Instantiate(config.Effect, transform.ThrowIfNull());

            SetCooldown(_stats.Cooldown);
            SetEffectShape();
        }

        public void LevelUp()
        {
            _level += Constants.One;
            _stats = _config.GetStats(_level);

            SetCooldown(_stats.Cooldown);
            SetEffectShape();
        }

        protected sealed override void Apply()
        {
            int count = Physics.OverlapSphereNonAlloc(GetPosition(), _stats.Range, _colliders, _config.DamageLayer);
            Debug.Log(count);

            for (int i = Constants.Zero; i < count; i++)
            {
                Collider collider = _colliders[i];

                if (collider.TryGetComponent(out Health health) == false)
                {
#if UNITY_EDITOR
                    Debug.Log("В слое врагов у кого то нет компонента здоровья");
#endif
                    continue;
                }

                health.TakeDamage(_stats.Damage);
            }

            _swingEffect.Play();
        }

        private void SetEffectShape()
        {
            ParticleSystem.ShapeModule shapeModule = _swingEffect.shape;
            shapeModule.radius = _stats.Range;
        }
    }
}
