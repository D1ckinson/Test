using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    [RequireComponent(typeof(Collider))]
    public class DamageSource : MonoBehaviour
    {
        [SerializeField] private DamageSourceConfig _config;

        private readonly Dictionary<Collider, Health> _damageable = new();
        private readonly List<Collider> _notDamageable = new();

#if UNITY_EDITOR
        private void OnValidate()
        {
            _config.ThrowIfNull();
        }
#endif

        private void OnCollisionEnter(Collision collision)
        {
            collision.ThrowIfNull();
            HandleCollider(collision.collider);
        }

        private void OnCollisionStay(Collision collision)
        {
            collision.ThrowIfNull();
            HandleCollider(collision.collider);
        }

        private void HandleCollider(Collider collision)
        {
            if (_notDamageable.Contains(collision))
            {
                return;
            }

            if (_damageable.TryGetValue(collision, out Health knownHealth))
            {
                knownHealth.TakeDamage(_config.Value);

                return;
            }

            if (collision.TryGetComponent(out Health health) == false)
            {
                _notDamageable.Add(collision);

                return;
            }

            if (_config.CanDamage(health.EntityType) == false)
            {
                _notDamageable.Add(collision);

                return;
            }

            _damageable.Add(collision, health);
            health.TakeDamage(_config.Value);
        }

    }
}
