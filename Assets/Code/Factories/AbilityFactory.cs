using Assets.Code.AbilitySystem;
using Assets.Code.AbilitySystem.Abilities;
using Assets.Code.Tools;
using Assets.Scripts.Factories;
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
        private readonly Dictionary<AbilityType, int> _abilityUnlockLevel;
        private readonly LootFactory _lootFactory;

        public AbilityFactory(Dictionary<AbilityType, AbilityConfig> configs, Transform hero, Transform swingEffectPoint, Dictionary<AbilityType, int> abilityUnlockLevel, LootFactory lootFactory)
        {
            _configs = configs.ThrowIfNullOrEmpty();
            _hero = hero.ThrowIfNull();

            _swingEffectPoint = swingEffectPoint.ThrowIfNull();
            _abilityUnlockLevel = abilityUnlockLevel.ThrowIfNull();
            _lootFactory = lootFactory.ThrowIfNull();

            _createFunctions = new()
            {
                [AbilityType.SwordStrike] = CreateSwordStrike,
                [AbilityType.GhostSwords] = CreateGhostSwords,
                [AbilityType.HolyGround] = CreateHolyGround,
                [AbilityType.MidasHand] = CreateMidasHand,
            };
        }

        public Ability Create(AbilityType abilityType)
        {
            return _createFunctions[abilityType].Invoke();
        }

        private SwordStrike CreateSwordStrike()
        {
            AbilityConfig abilityConfig = _configs[AbilityType.SwordStrike];

            return new SwordStrike(abilityConfig, _swingEffectPoint, _hero.GetComponentOrThrow<Animator>(), _abilityUnlockLevel);
        }

        private Ability CreateGhostSwords()
        {
            AbilityConfig abilityConfig = _configs[AbilityType.GhostSwords];

            return new GhostSwords(abilityConfig, _swingEffectPoint, _abilityUnlockLevel);
        }

        private Ability CreateHolyGround()
        {
            AbilityConfig abilityConfig = _configs[AbilityType.HolyGround];

            return new HolyGround(abilityConfig, _hero, _abilityUnlockLevel);
        }

        private Ability CreateMidasHand()
        {
            AbilityConfig abilityConfig = _configs[AbilityType.MidasHand];

            return new MidasHand(abilityConfig, _hero, _abilityUnlockLevel, _lootFactory);
        }
    }
}