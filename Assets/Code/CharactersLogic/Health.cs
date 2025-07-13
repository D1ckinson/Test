using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Health : MonoBehaviour
    {
        private float _invincibleTimer;
        private float _invincibilityDuration;

        public event Action GetHit;
        public event Action Died;
        public event Action<float> ValueChanged;

        public float Value { get; private set; }
        public float MaxValue { get; private set; }
        private bool IsInvincible => _invincibleTimer > Constants.Zero;

        private void OnEnable()
        {
            SetMaxValue();
        }

        private void Update()
        {
            if (IsInvincible)
            {
                _invincibleTimer -= Time.deltaTime;
            }
        }

        public void Initialize(float maxValue, float invincibilityDuration)
        {
            MaxValue = maxValue.ThrowIfZeroOrLess();
            Value = MaxValue;
            _invincibilityDuration = invincibilityDuration.ThrowIfNegative();
        }

        private void SetMaxValue()
        {
            Value = MaxValue;
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
                gameObject.SetActive(false);
            }
            else
            {
                Value = tempValue;
                _invincibleTimer = _invincibilityDuration;
            }

            ValueChanged?.Invoke(Value);
        }
    }
}
