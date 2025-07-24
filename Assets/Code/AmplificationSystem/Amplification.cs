using Assets.Code.Data.Interfaces;
using Assets.Scripts;
using Assets.Scripts.Movement;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.AmplificationSystem
{
    public abstract class Amplification<T>
    {
        private readonly Dictionary<int, int> _valueOnLevel;
        private readonly List<T> _components;

        public Amplification(Dictionary<int, int> valueOnLevel, T component)
        {
            _valueOnLevel = valueOnLevel.ThrowIfCollectionNullOrEmpty();
            _components = new()
            {
                component.ThrowIfNull()
            };

            Amplify(component, _valueOnLevel[Level]);
        }

        public int Level { get; private set; } = 1;

        public void Add(T component)
        {
            _components.Add(component.ThrowIfNull());
            Amplify(component, _valueOnLevel[Level]);
        }

        public void LevelUp()
        {
            Level++;
            AmplifyAll();
        }

        protected abstract void Amplify(T component, int value);

        private void AmplifyAll()
        {
            int value = _valueOnLevel[Level];

            foreach (T component in _components)
            {
                Amplify(component, value);
            }
        }
    }

    public class HealthAmplification : Amplification<Health>
    {
        public HealthAmplification(Dictionary<int, int> valueOnLevel, Health health) : base(valueOnLevel, health) { }

        protected override void Amplify(Health health, int value)
        {
            health.SetAdditionalValue(value);
        }
    }

    public class RegenAmplification : Amplification<Health>
    {
        public RegenAmplification(Dictionary<int, int> valueOnLevel, Health health) : base(valueOnLevel, health) { }

        protected override void Amplify(Health health, int value)
        {
            health.SetRegeneration(value);
        }
    }

    public class DamageAmplification : Amplification<Ability>
    {
        public DamageAmplification(Dictionary<int, int> valueOnLevel, Ability ability) : base(valueOnLevel, ability) { }

        protected override void Amplify(Ability ability, int value)
        {
            ability.SetAdditionalDamage(value);
        }
    }

    public class CooldownAmplification : Amplification<Ability>
    {
        public CooldownAmplification(Dictionary<int, int> valueOnLevel, Ability ability) : base(valueOnLevel, ability) { }

        protected override void Amplify(Ability ability, int percent)
        {
            ability.SetCooldownPercent(percent);
        }
    }

    public class SpeedAmplification : Amplification<CharacterMovement>
    {
        public SpeedAmplification(Dictionary<int, int> valueOnLevel, CharacterMovement characterMovement) : base(valueOnLevel, characterMovement) { }

        protected override void Amplify(CharacterMovement characterMovement, int value)
        {
            characterMovement.SetAdditionalSpeed(value);
        }
    }

    public class LootAmplification : Amplification<Wallet>
    {
        public LootAmplification(Dictionary<int, int> valueOnLevel, Wallet valueContainer) : base(valueOnLevel, valueContainer) { }

        protected override void Amplify(Wallet valueContainer, int percent)
        {
            valueContainer.SetLootPercent(percent);
        }
    }

    public class LootAttractionRadiusAmplification : Amplification<LootCollector>
    {
        public LootAttractionRadiusAmplification(Dictionary<int, int> valueOnLevel, LootCollector lootCollector) : base(valueOnLevel, lootCollector) { }

        protected override void Amplify(LootCollector lootCollector, int value)
        {
            lootCollector.AddAttractionRadius(value);
        }
    }

    public class ResistAmplification : Amplification<Health>
    {
        public ResistAmplification(Dictionary<int, int> valueOnLevel, Health health) : base(valueOnLevel, health) { }

        protected override void Amplify(Health health, int resistPercent)
        {
            health.SetResistPercent(resistPercent);
        }
    }

    public class ExperienceAmplification : Amplification<HeroExperience>
    {
        public ExperienceAmplification(Dictionary<int, int> valueOnLevel, HeroExperience heroExperience) : base(valueOnLevel, heroExperience) { }

        protected override void Amplify(HeroExperience heroExperience, int additionalExperiencePercent)
        {
            heroExperience.SetLootPercent(additionalExperiencePercent);
        }
    }

    public class AmplificationFactory
    {
        public void Create()
        {

        }
    }

    [Serializable]
    public struct AmplificationConfig
    {
        [SerializeField] private int _amplificationValue;

    }
}
