using Assets.Code.Tools;
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

        private readonly Transform _swingEffectPoint;

        public AbilityFactory(Dictionary<AbilityType, AbilityConfig> configs, Transform hero, Transform swingEffectPoint)
        {
            _configs = configs.ThrowIfNullOrEmpty();
            _hero = hero.ThrowIfNull();

            _swingEffectPoint = swingEffectPoint;

            _createFunctions = new()
            {
                [AbilityType.SwordStrike] = CreateSwordStrike
            };
        }

        public Ability Create(AbilityType abilityType)
        {
            return _createFunctions[abilityType].Invoke();
        }

        private SwordStrike CreateSwordStrike()
        {
            AbilityConfig abilityConfig = _configs[AbilityType.SwordStrike];

            return new SwordStrike(abilityConfig, _swingEffectPoint, _hero.GetComponentOrThrow<Animator>());
        }
    }
}