using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class AbilityFactory
    {
        private readonly Dictionary<AbilityType, AbilityConfig> _configs;
        private readonly Dictionary<AbilityType, Func<Ability>> _createFunctions;
        private readonly Transform _hero;

        public AbilityFactory(Dictionary<AbilityType, AbilityConfig> configs, Transform hero)
        {
            _configs = configs.ThrowIfNullOrEmpty();
            _hero = hero.ThrowIfNull();

            _createFunctions = new()
            {
                [AbilityType.SwordStrike] = CreateSwordStrike
            };
        }

        public Ability Create(AbilityType abilityType)
        {
            _createFunctions.TryGetValue(abilityType.ThrowIfNull(), out Func<Ability> createFunc).ThrowIfFalse();

            return createFunc.Invoke();
        }

        private SwordStrike CreateSwordStrike()
        {
            AbilityConfig abilityConfig = _configs[AbilityType.SwordStrike];

            return new SwordStrike(abilityConfig, _hero);
        }
    }
}