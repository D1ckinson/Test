using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Code
{
    public abstract class Ability
    {
        private readonly Transform _transform;
        private readonly AbilityConfig _config;

        private float _maxCooldown;
        private float _currentCooldown;
        private float _additionalDamage;
        private float _cooldownMultiplier = 1f;

        protected Ability(AbilityConfig config, Transform transform, int level = 1)
        {
            _config = config.ThrowIfNull();
            _transform = transform.ThrowIfNull();
            Level = level.ThrowIfZeroOrLess().ThrowIfMoreThan(_config.MaxLevel);

            _maxCooldown = _config.GetStats(Level).Cooldown;
            _currentCooldown = _maxCooldown;
        }

        public AbilityType Type => _config.Type;
        public int Level { get; private set; }
        public bool IsMaxed => Level == _config.MaxLevel;

        public void Update()
        {
            _currentCooldown -= Time.deltaTime;

            if (_currentCooldown < Constants.Zero)
            {
                Apply();
                _currentCooldown = _maxCooldown;
            }
        }

        public void LevelUp()
        {
            Level++;
            AbilityStats stats = _config.GetStats(Level);

            _maxCooldown = stats.Cooldown;
            UpdateStats(stats.Damage + _additionalDamage, stats.Range, stats.ProjectilesCount);
        }

        protected Vector3 GetPosition()
        {
            return _transform.position;
        }

        protected abstract void UpdateStats(float damage, float range, float projectilesCount);

        protected abstract void Apply();

        public void SetAdditionalDamage(int value)
        {
            _additionalDamage = value.ThrowIfNegative();

            AbilityStats stats = _config.GetStats(Level);
            UpdateStats(stats.Damage + _additionalDamage, stats.Range, stats.ProjectilesCount);
        }

        public void SetCooldownPercent(float percent)
        {
            _cooldownMultiplier = Constants.PercentToMultiplier(percent.ThrowIfNegative());
            _maxCooldown *= _cooldownMultiplier;
        }
    }
}
