using Assets.Code.CharactersLogic;
using Assets.Code.Loot;
using Assets.Code.Tools;
using Assets.Scripts.Factories;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.AbilitySystem.Abilities
{
    public class MidasHand : Ability
    {
        private readonly LayerMask _damageLayer;
        private readonly Collider[] _colliders = new Collider[50];
        private readonly Transform _transform;
        private readonly LootFactory _lootFactory;

        private float _damage;
        private float _range;
        private int _healthPercent;

        public MidasHand(AbilityConfig config, Transform transform, Dictionary<AbilityType, int> abilityUnlockLevel, LootFactory lootFactory, int level = 1) : base(config, transform, abilityUnlockLevel, level)
        {
            _damageLayer = config.DamageLayer.ThrowIfNull();
            _transform = transform.ThrowIfNull();
            _lootFactory = lootFactory.ThrowIfNull();

            AbilityStats stats = config.ThrowIfNull().GetStats(level);

            _damage = stats.Damage.ThrowIfNegative();
            _range = stats.Range.ThrowIfNegative();
            _healthPercent = stats.HealthPercent.ThrowIfNegative();
        }

        public override void Dispose() { }

        protected override void Apply()
        {
            int count = Physics.OverlapSphereNonAlloc(Position, _range, _colliders, _damageLayer);
            float distance = float.MaxValue;
            Collider closest = null;

            for (int i = Constants.Zero; i < count; i++)
            {
                Collider collider = _colliders[i];
                float sqrDistance = (collider.transform.position - _transform.position).sqrMagnitude;

                if (sqrDistance < distance)
                {
                    closest = collider;
                    distance = sqrDistance;
                }
            }

            if (closest.NotNull() && closest.TryGetComponent(out Health health))
            {
                health.TakeDamage(_damage);
                float floatPercent = (float)_healthPercent / Constants.Hundred;
                int coinsCount = (int)(health.MaxValue * floatPercent);
                Debug.Log(coinsCount);
                _lootFactory.Spawn(LootType.Coin, closest.transform.position, coinsCount);
            }
        }

        protected override void UpdateStats(float damage, float range, int projectilesCount, bool isPiercing, int healthPercent)
        {
            _damage = damage.ThrowIfNegative();
            _range = range.ThrowIfNegative();
            _healthPercent = healthPercent.ThrowIfNegative();
        }
    }
}
