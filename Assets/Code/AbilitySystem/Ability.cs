using Assets.Code.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.AbilitySystem
{
    public abstract class Ability : IDisposable
    {
        private readonly Transform _transform;
        private readonly AbilityConfig _config;
        private readonly Dictionary<AbilityType, int> _abilityUnlockLevel;

        private float _cooldown;
        private float _currentCooldown;
        private float _additionalDamage;
        private float _cooldownMultiplier = 1f;

        protected Ability(AbilityConfig config, Transform transform, Dictionary<AbilityType, int> abilityUnlockLevel, int level = 1)
        {
            _config = config.ThrowIfNull();
            _transform = transform.ThrowIfNull();
            Level = level.ThrowIfZeroOrLess().ThrowIfMoreThan(_config.MaxLevel);
            _abilityUnlockLevel = abilityUnlockLevel.ThrowIfNullOrEmpty();

            _cooldown = _config.GetStats(Level).Cooldown;
            _currentCooldown = _cooldown;
        }

        public AbilityType Type => _config.Type;
        public int Level { get; private set; }
        public bool IsMaxed => Level == _abilityUnlockLevel[_config.Type];
        protected Vector3 Position => _transform.position;

        public void Run()
        {
            UpdateService.RegisterUpdate(Update);
        }

        public void Stop()
        {
            UpdateService.UnregisterUpdate(Update);
        }

        public void Update()
        {
            _currentCooldown -= Time.deltaTime;

            if (_currentCooldown < Constants.Zero)
            {
                Apply();
                _currentCooldown = _cooldown;
            }
        }

        public void LevelUp()
        {
            Level++;
            AbilityStats stats = _config.GetStats(Level);

            _cooldown = stats.Cooldown;
            UpdateStats(stats.Damage + _additionalDamage, stats.Range, stats.ProjectilesCount, stats.IsPiercing, stats.HealthPercent, stats.PullForce);
        }

        public void SetAdditionalDamage(int value)
        {
            _additionalDamage = value.ThrowIfNegative();

            AbilityStats stats = _config.GetStats(Level);
            UpdateStats(stats.Damage + _additionalDamage, stats.Range, stats.ProjectilesCount, stats.IsPiercing, stats.HealthPercent, stats.PullForce);
        }

        public void SetCooldownPercent(float percent)
        {
            _cooldownMultiplier = Constants.PercentToMultiplier(percent.ThrowIfNegative());
            _cooldown *= _cooldownMultiplier;
        }

        public abstract void Dispose();

        protected abstract void UpdateStats(float damage, float range, int projectilesCount, bool isPiercing, int healthPercent, float pullForce);

        protected abstract void Apply();
    }
}
