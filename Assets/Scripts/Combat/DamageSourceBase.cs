using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class DamageSourceBase<T> : MonoBehaviour where T : DamageSourceConfig
    {
        [SerializeField] protected T _config;//

        private readonly Dictionary<GameObject, Health> _damageable = new();
        private readonly List<GameObject> _notDamageable = new();

#if UNITY_EDITOR
        private void OnValidate()
        {
            _config.ThrowIfNull();
        }
#endif

        protected void TryDamage(GameObject gameObject)
        {
            if (_notDamageable.Contains(gameObject))
            {
                return;
            }

            if (_damageable.TryGetValue(gameObject, out Health health))
            {
                health.TakeDamage(_config.Damage);

                return;
            }

            if (gameObject.TryGetComponent(out health) == false)
            {
                _notDamageable.Add(gameObject);

                return;
            }

            if (_config.CanDamage(health.EntityType) == false)
            {
                _notDamageable.Add(gameObject);

                return;
            }

            _damageable.Add(gameObject, health);
            health.TakeDamage(_config.Damage);
        }
    }
}
