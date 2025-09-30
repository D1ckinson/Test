using Assets.Code.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.AbilitySystem.Abilities
{
    public class HolyGround : Ability
    {
        private readonly HolyRune _holyRune;

        public HolyGround(AbilityConfig config, Transform transform, Dictionary<AbilityType, int> abilityUnlockLevel, int level = 1) : base(config, transform, abilityUnlockLevel, level)
        {
            _holyRune = config.ThrowIfNull().ProjectilePrefab.GetComponentOrThrow<HolyRune>().Instantiate();

            AbilityStats stats = config.GetStats(level);
            _holyRune.Initialize(stats.Damage, stats.Range, config.DamageLayer, transform);
        }

        protected override void Apply()
        {
            _holyRune.DealDamage();
        }

        protected override void UpdateStats(float damage, float range, int projectilesCount, bool isPiercing)
        {
            _holyRune.SetStats(damage, range);
        }

        public override void Dispose()
        {
            _holyRune.DestroyGameObject();
        }
    }
}
