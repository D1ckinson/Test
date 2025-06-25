using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.Combat.Abilities
{
    internal class SwordStrike : MonoBehaviour
    {
        private const int MaxCountForStrike = 50;

        [SerializeField] private SwordStrikeStats _stats;
        [SerializeField] private ParticleSystem _swingEffect;

        private Transform _transform;
        private float _cooldownTimer;
        private Collider[] _colliders;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_stats != null)
            {
                CustomGizmos.DrawCircle(transform.position, _stats.Radius);
            }
        }

        private void OnValidate()
        {
            _stats.ThrowIfNull();
            _swingEffect.ThrowIfNull();
        }
#endif

        private void Awake()
        {
            _transform = transform;
            _colliders = new Collider[MaxCountForStrike];
            _cooldownTimer = _stats.Cooldown;
            SetEffectShape();
        }

        private void Update()
        {
            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= Constants.Zero)
            {
                Strike();
                _cooldownTimer = _stats.Cooldown;
            }
        }

        private void Strike()
        {
            int count = Physics.OverlapSphereNonAlloc(_transform.position, _stats.Radius, _colliders);

            for (int i = Constants.Zero; i < count; i++)
            {
                if (_colliders[i].TryGetComponent(out Health health) == false)
                {
                    continue;
                }

                if (health.EntityType == EntityType.Enemy)
                {
                    health.TakeDamage(_stats.Damage);
                }
            }

            _swingEffect.Play();
        }

        private void SetEffectShape()
        {
            ParticleSystem.ShapeModule shapeModule = _swingEffect.shape;
            shapeModule.radius = _stats.Radius;
        }
    }
}