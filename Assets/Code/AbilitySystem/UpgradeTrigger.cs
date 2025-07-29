using Assets.Scripts;
using Assets.Scripts.Tools;
using Assets.Scripts.Ui;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using Assets.Code.Tools;

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
            _levelUpWindow.UpgradeChosen += UpgradeAbility;
        }

        ~UpgradeTrigger()
        {
            if (_heroExperience.IsNull() == false)
            {
                _heroExperience.LevelUp -= GenerateUpgrades;
            }

            if (_levelUpWindow.IsNull() == false)
            {
                _levelUpWindow.UpgradeChosen -= UpgradeAbility;
            }
        }

        private void GenerateUpgrades(int level)
        {
            List<AbilityType> possibleUpgrades = Constants.GetEnums<AbilityType>().Except(_abilityContainer.MaxedAbilities).ToList();
            List<UpgradeOption> upgradeOptions = new();

            for (int i = Constants.Zero; i < SuggestedUpgradesCount; i++)
            {
                if (possibleUpgrades.Count == Constants.Zero)
                {
                    break;
                }

                int index = Random.Range(Constants.Zero, possibleUpgrades.GetLastIndex());
                AbilityType abilityType = possibleUpgrades[index];
                possibleUpgrades.RemoveAt(index);

                int abilityLevel = _abilityContainer.GetAbilityLevel(abilityType);

                AbilityConfig abilityConfig = _abilityConfigs[abilityType];
                AbilityStats currentStats = abilityConfig.GetStats(abilityLevel);
                List<string> statsDescription;

                if (abilityLevel > Constants.Zero)
                {
                    AbilityStats nextStats = abilityConfig.GetStats(abilityLevel + Constants.One);
                    AbilityStats StatsDifference = nextStats - currentStats;
                    statsDescription = StatsDifference.GetStatsDescription();
                }
                else
                {
                    statsDescription = currentStats.GetStatsDescription();
                }

                upgradeOptions.Add(new(abilityType, abilityLevel, statsDescription, abilityConfig.Image));
            }

            if (upgradeOptions.Count == Constants.Zero)
            {
                //Наградить

                return;
            }

            _levelUpWindow.Show(upgradeOptions, level.ThrowIfNegative());
        }

        private void UpgradeAbility(AbilityType abilityType)
        {
            abilityType.ThrowIfNull();

            switch (_abilityContainer.HasAbility(abilityType))
            {
                case true:
                    _abilityContainer.Upgrade(abilityType);
                    break;

                case false:
                    _abilityContainer.Add(_abilityFactory.Create(abilityType));
                    break;
            }
        }
    }
}
