using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Code
{
    public abstract class Ability
    {
        public readonly AbilityType Type;

        private readonly Transform _transform;

        private float _maxCooldown;
        private float _currentCooldown;

        protected Ability(AbilityType type, Transform transform)
        {
            Type = type.ThrowIfNull();
            _transform = transform.ThrowIfNull();
            _currentCooldown = _maxCooldown;
        }

        public event Action Strike;

        public void Update()
        {
            _currentCooldown -= Time.deltaTime;

            if (_currentCooldown < Constants.Zero)
            {
                Apply();
                _currentCooldown = _maxCooldown;
            }
        }

        protected void SetCooldown(float cooldown)
        {
            _maxCooldown = cooldown.ThrowIfNegative();
        }

        protected Vector3 GetPosition()
        {
            return _transform.position;
        }

        protected abstract void Apply();
    }
}
