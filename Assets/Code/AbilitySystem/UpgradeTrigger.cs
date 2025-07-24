using Assets.Code.AmplificationSystem;
using Assets.Code.Tools;
using Assets.Scripts;
using Assets.Scripts.Tools;
using Assets.Scripts.Ui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.AbilitySystem
{
    public class UpgradeTrigger
    {
        private const int SuggestedUpgradesCount = 3;

        private readonly HeroExperience _heroExperience;
        private readonly AbilityContainer _abilityContainer;
        private readonly LevelUpWindow _levelUpWindow;
        private readonly AbilityFactory _abilityFactory;
        private readonly BuffFactory _buffFactory;
        private readonly BuffContainer _buffContainer;
        //private readonly Dictionary<UpgradeType, IUpgradeContainer> _containers;

        public UpgradeTrigger
            (HeroExperience heroExperience, AbilityContainer abilityContainer, LevelUpWindow levelUpWindow,
            AbilityFactory abilityFactory, BuffFactory buffFactory, BuffContainer buffContainer)
        {
            _heroExperience = heroExperience.ThrowIfNull();
            _abilityContainer = abilityContainer.ThrowIfNull();
            _levelUpWindow = levelUpWindow.ThrowIfNull();
            _abilityFactory = abilityFactory.ThrowIfNull();
            _buffFactory = buffFactory.ThrowIfNull();
            _buffContainer = buffContainer.ThrowIfNull();

            //_containers = new()
            //{
            //    [UpgradeType.Ability] = (IUpgradeContainer)abilityContainer.ThrowIfNull(),
            //    [UpgradeType.Buff] = (IUpgradeContainer)buffContainer.ThrowIfNull()
            //};

            _heroExperience.LevelUp += GenerateUpgrades;
        }

        ~UpgradeTrigger()
        {
            if (_heroExperience != null)
            {
                _heroExperience.LevelUp -= GenerateUpgrades;
            }
        }

        private void GenerateUpgrades(int level)
        {
            List<UpgradeOption> upgrades = new();

            IEnumerable<AbilityType> abilityTypes = Constants.AbilityTypes.Except(_abilityContainer.MaxedAbilities);
            IEnumerable<BuffType> buffTypes = Constants.BuffTypes.Except(_buffContainer.MaxedBuffs);

            foreach (AbilityType type in abilityTypes)
            {
                UpgradeOption upgrade = new(UpgradeType.Ability, type);
                upgrades.Add(upgrade);
            }

            foreach (BuffType type in buffTypes)
            {
                UpgradeOption upgrade = new(UpgradeType.Buff, type);
                upgrades.Add(upgrade);
            }

            List<UpgradeOption> pickedUpgrades = new();

            for (int i = Constants.Zero; i < SuggestedUpgradesCount; i++)
            {
                int index = Random.Range(Constants.Zero, upgrades.GetLastIndex());
                UpgradeOption option = upgrades[index];

                pickedUpgrades.Add(option);
                upgrades.Remove(option);
            }

            for (int i = Constants.Zero; i < pickedUpgrades.Count; i++)
            {
                UpgradeOption option = pickedUpgrades[i];

                if (option.Type == UpgradeType.Ability)
                {
                    AbilityType type = (AbilityType)option.SpecificType;

                    if (_abilityContainer.HasUpgrade(type))
                    {
                        option.NextLevel = _abilityContainer.GetAbilityLevel(type) + Constants.One;
                    }
                    else
                    {
                        option.NextLevel = Constants.One;
                    }

                    AbilityConfig config = _abilityFactory.GetConfig(type);
                    option.Icon = config.Icon;
                    option.Description = WriteDescription(config, option.NextLevel);
                }
                else if (option.Type == UpgradeType.Buff)
                {
                    BuffType type = (BuffType)option.SpecificType;

                    if (_buffContainer.HasAbility(type))
                    {
                        option.NextLevel = _buffContainer.GetAbilityLevel(type) + Constants.One;
                    }
                    else
                    {
                        option.NextLevel = Constants.One;
                    }

                    BuffConfig config = _buffFactory.GetConfig(type);
                    option.Icon = config.Icon;
                    option.Description = WriteDescription(config, option.NextLevel);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }


            if (upgrades.IsEmpty())
            {
                //Наградить

                return;
            }

            _levelUpWindow.Show(upgrades, level);
            _levelUpWindow.UpgradeChosenEnum += Upgrade;
        }

        private void Upgrade(Enum @enum)
        {
            switch (@enum)
            {
                case AbilityType:
                    UpgradeAbility((AbilityType)@enum);
                    break;

                case BuffType:
                    UpgradeBuff((BuffType)@enum);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void UpgradeBuff(BuffType type)
        {
            type.ThrowIfNull();

            if (_buffContainer.HasUpgrade(type))
            {
                _buffContainer.IsMaxed(type).ThrowIfTrue(new NotImplementedException());
                _buffContainer.Upgrade(type);/////////////////////////////

                return;
            }

            _buffContainer.Add(_buffFactory.Create(type));

        }

        private string WriteDescription(BuffConfig config, int nextLevel)
        {
            return "ЗАГЛУШКА";
        }

        private string WriteDescription(AbilityConfig config, int nextLevel)
        {
            return "ЗАГЛУШКА";
        }

        private void UpgradeAbility(AbilityType type)
        {
            type.ThrowIfNull();

            if (_abilityContainer.HasUpgrade(type))
            {
                _abilityContainer.IsMaxed(type).ThrowIfTrue(new NotImplementedException());
                _abilityContainer.Upgrade(type);

                return;
            }

            _abilityContainer.Add(_abilityFactory.Create(type));
        }
    }

    public interface IUpgradeContainer<T> : IUpgradeContainer where T : Enum
    {
        public IEnumerable<T> Maxed { get; }
        public bool HasUpgrade(T type);
    }

    public interface IUpgradeContainer
    {
        public IEnumerable Maxed { get; }

        public bool HasUpgrade(Enum type);
    }

    public struct UpgradeOption
    {
        public UpgradeType Type;
        public Enum SpecificType;
        public int NextLevel;
        public Sprite Icon;
        public string Description;

        public UpgradeOption(UpgradeType type, Enum specificType) : this()
        {
            Type = type.ThrowIfNull();
            SpecificType = specificType.ThrowIfNull();
        }
    }

    public enum UpgradeType
    {
        Default,
        Ability,
        Buff
    }
}
