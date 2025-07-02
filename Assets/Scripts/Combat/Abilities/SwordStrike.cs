using Assets.Scripts.Tools;
using Assets.Scripts.Movement;
using UnityEngine;

namespace Assets.Scripts.Combat.Abilities
{
    internal class SwordStrike : DamageSourceBase<SwordStrikeConfig>
    {
        private const int MaxCountForStrike = 50;

        [SerializeField] private ParticleSystem _swingEffect;

        private Transform _transform;
        private float _cooldownTimer;
        private Collider[] _colliders;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_config != null)
            {
                CustomGizmos.DrawCircle(transform.position, _config.Radius);
            }
        }

        private void OnValidate()
        {
            _swingEffect.ThrowIfNull();
        }
#endif

        private void Awake()
        {
            _transform = transform;
            _colliders = new Collider[MaxCountForStrike];

            _cooldownTimer = _config.Cooldown;
            SetEffectShape();
        }

        private void Update()
        {
            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= Constants.Zero)
            {
                Strike();
                _cooldownTimer = _config.Cooldown;
            }
        }

        private void Strike()
        {
            int count = Physics.OverlapSphereNonAlloc(_transform.position, _config.Radius, _colliders);

            for (int i = Constants.Zero; i < count; i++)
            {
                Collider collider = _colliders[i];

                TryDamage(collider.gameObject);
            }

            _swingEffect.Play();
        }

        private void SetEffectShape()
        {
            ParticleSystem.ShapeModule shapeModule = _swingEffect.shape;
            shapeModule.radius = _config.Radius;
        }
    }
}
