using Assets.Scripts;
using Assets.Scripts.Tools;
using Assets.Scripts.Ui;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.AbilitySystem
{
    public class UpgradeTrigger
    {
        private const int SuggestedUpgradesCount = 3;

        private readonly HeroExperience _heroExperience;
        private readonly Dictionary<AbilityType, AbilityConfig> _abilityConfigs;
        private readonly AbilityContainer _abilityContainer;
        private readonly LevelUpWindow _levelUpWindow;
        private readonly AbilityFactory _abilityFactory;

        public UpgradeTrigger
            (HeroExperience heroExperience, Dictionary<AbilityType, AbilityConfig> abilityConfigs,
            AbilityContainer abilityContainer, LevelUpWindow levelUpWindow, AbilityFactory abilityFactory)
        {
            _heroExperience = heroExperience.ThrowIfNull();
            _abilityContainer = abilityContainer.ThrowIfNull();
            _abilityConfigs = abilityConfigs.ThrowIfCollectionNullOrEmpty();
            _levelUpWindow = levelUpWindow.ThrowIfNull();
            _abilityFactory = abilityFactory.ThrowIfNull();

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
            List<AbilityType> maxedAbilities = _abilityContainer.GetMaxedAbilities();
            List<AbilityType> possibleUpgrades = _abilityConfigs.Keys.Except(maxedAbilities).ToList();
            List<AbilityType> suggestedUpgrades = new();

            for (int i = Constants.Zero; i < SuggestedUpgradesCount; i++)
            {
                if (possibleUpgrades.Count == Constants.Zero)
                {
                    break;
                }

                int index = Random.Range(Constants.Zero, possibleUpgrades.Count - Constants.One);
                suggestedUpgrades.Add(possibleUpgrades[index]);
                possibleUpgrades.RemoveAt(index);
            }

            Dictionary<AbilityConfig, int> upgrades = new();

            for (int i = Constants.Zero; i < suggestedUpgrades.Count; i++)
            {
                AbilityType abilityType = suggestedUpgrades[i];
                int abilityNextLevel;

                if (_abilityContainer.HasAbility(abilityType))
                {
                    abilityNextLevel = _abilityContainer.GetAbilityLevel(abilityType) + Constants.One;
                }
                else
                {
                    abilityNextLevel = Constants.One;
                }

                upgrades.Add(_abilityConfigs[abilityType], abilityNextLevel);
            }

            if (upgrades.Count == Constants.Zero)
            {
                //Наградить

                return;
            }

            _levelUpWindow.Show(upgrades, level.ThrowIfNegative());
            _levelUpWindow.UpgradeChosen += UpgradeAbility;
        }

        private void UpgradeAbility(AbilityType abilityType)
        {
            abilityType.ThrowIfNull();

            if (_abilityContainer.HasAbility(abilityType))
            {
                if (_abilityContainer.IsMaxed(abilityType))
                {
                    return;
                }

                _abilityContainer.Upgrade(abilityType);

                return;
            }

            _abilityContainer.Add(_abilityFactory.Create(abilityType));
        }
    }
}
