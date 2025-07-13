using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class AbilityFactory
    {
        private readonly Dictionary<AbilityType, Func<NewAbilityConfig, Transform, Ability>> _abilityCreators;
        private readonly Dictionary<AbilityType, NewAbilityConfig> _configs = new();

        public AbilityFactory(List<NewAbilityConfig> configs)
        {
            configs.ThrowIfCollectionNull();
            configs.ForEach(config => _configs.Add(config.Type, config));


            _abilityCreators = new Dictionary<AbilityType, Func<NewAbilityConfig, Transform, Ability>>
            {
                { AbilityType.SwordStrike, (config, transform) => new NewSwordStrike(config,transform)}
                // Добавь другие типы способностей здесь
            };
        }

        public T Create<T>(NewAbilityConfig abilityConfig, Transform hero) where T : Ability
        {
            abilityConfig.ThrowIfNull();
            _abilityCreators.TryGetValue(abilityConfig.Type, out Func<NewAbilityConfig, Transform, Ability> creator).ThrowIfFalse(new NotImplementedException());

            T ability = creator(abilityConfig, hero.ThrowIfNull()) as T;

            return ability.ThrowIfNull();
        }

        public NewSwordStrike CreateSwordStrike(Transform hero)
        {
            NewAbilityConfig abilityConfig = _configs[AbilityType.SwordStrike];

            return new NewSwordStrike(abilityConfig, hero.ThrowIfNull());
        }

        private ParticleSystem LoadSwingEffect(AbilityType type)
        {
            // Загрузка префаба эффекта по типу способности
            var path = $"Effects/{type}SwingEffect";
            var prefab = Resources.Load<ParticleSystem>(path);
            if (prefab == null)
                throw new MissingReferenceException($"Swing effect not found at path: {path}");

            return UnityEngine.Object.Instantiate(prefab);
        }
    }
}