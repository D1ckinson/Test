using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private HealthConfig _config;
        [field: SerializeField] public EntityType EntityType { get; private set; }

        private float _invincibleTimer;

        public event Action GetHit;
        public event Action Died;
        public event Action<float> ValueChanged;

        public float Value { get; private set; }
        public float MaxValue => _config.MaxValue;
        private bool IsInvincible => _invincibleTimer >= Constants.Zero;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _config.ThrowIfNull();
            EntityType.ThrowIfNull();
        }
#endif

        private void Update()
        {
            if (IsInvincible)
            {
                _invincibleTimer -= Time.deltaTime;
            }
        }

        public void SetMaxValue()
        {
            Value = _config.MaxValue;
            ValueChanged?.Invoke(Value);
        }

        public void TakeDamage(float damage)
        {
            if (IsInvincible)
            {
                return;
            }

            damage.ThrowIfZeroOrLess();

            float tempValue = Value - damage;
            GetHit?.Invoke();

            if (tempValue <= Constants.Zero)
            {
                Value = Constants.Zero;
                Died?.Invoke();
            }
            else
            {
                Value = tempValue;
                _invincibleTimer = _config.InvincibilityDuration;
            }

            ValueChanged?.Invoke(Value);
        }
    }
}