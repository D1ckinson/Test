using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts
{
    internal class SwordStrike : MonoBehaviour
    {
        private const int MaxCountForStrike = 50;

        [SerializeField] private ParticleSystem _swingEffect;
        [SerializeField] private AbilityConfig _config;

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
            _config.ThrowIfNull();
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
            int count = Physics.OverlapSphereNonAlloc(_transform.position, _config.Radius, _colliders, _config.DamageLayer);

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

                health.TakeDamage(_config.Damage);
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
