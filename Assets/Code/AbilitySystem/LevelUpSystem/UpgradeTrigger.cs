using Assets.Scripts;
using Assets.Scripts.Ui;
using Assets.Code.Tools;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Assets.Code.AbilitySystem
{
    public class UpgradeTrigger
    {
        private const int SuggestedUpgradesCount = 3;

        private readonly HeroLevel _heroExperience;
        private readonly Dictionary<AbilityType, AbilityConfig> _abilityConfigs;
        private readonly AbilityContainer _abilityContainer;
        private readonly LevelUpWindow _levelUpWindow;
        private readonly AbilityFactory _abilityFactory;
        private readonly Dictionary<AbilityType, int> _abilityUnlockLevel;
        private readonly ITimeService _timeService;

        public UpgradeTrigger
            (HeroLevel heroExperience, Dictionary<AbilityType, AbilityConfig> abilityConfigs,
            AbilityContainer abilityContainer, LevelUpWindow levelUpWindow, AbilityFactory abilityFactory,
            Dictionary<AbilityType, int> abilityUnlockLevel, ITimeService timeService)
        {
            _heroExperience = heroExperience.ThrowIfNull();
            _abilityContainer = abilityContainer.ThrowIfNull();
            _abilityConfigs = abilityConfigs.ThrowIfNullOrEmpty();
            _levelUpWindow = levelUpWindow.ThrowIfNull();
            _abilityFactory = abilityFactory.ThrowIfNull();
            _abilityUnlockLevel = abilityUnlockLevel;
            _timeService = timeService.ThrowIfNull();

            _heroExperience.LevelRaised += GenerateUpgrades;
            _levelUpWindow.UpgradeChosen += UpgradeAbility;
        }

        ~UpgradeTrigger()
        {
            if (_heroExperience.NotNull())
            {
                _heroExperience.LevelRaised -= GenerateUpgrades;
            }

            if (_levelUpWindow.NotNull())
            {
                _levelUpWindow.UpgradeChosen -= UpgradeAbility;
            }
        }

        private void GenerateUpgrades(int level)
        {
            List<AbilityType> possibleUpgrades = GetPossibleUpgrades();
            List<UpgradeOption> upgradeOptions = new();

            for (int i = Constants.Zero; i < SuggestedUpgradesCount; i++)
            {
                if (possibleUpgrades.Count == Constants.Zero)
                {
                    break;
                }

                int index = Random.Range(Constants.Zero, possibleUpgrades.LastIndex());
                AbilityType abilityType = possibleUpgrades[index];
                possibleUpgrades.RemoveAt(index);

                int abilityLevel = _abilityContainer.GetAbilityLevel(abilityType);

                AbilityConfig abilityConfig = _abilityConfigs[abilityType];
                AbilityStats nextStats = abilityConfig.GetStats(abilityLevel + Constants.One);
                List<string> statsDescription;

                if (abilityLevel > Constants.Zero)
                {
                    statsDescription = (nextStats - abilityConfig.GetStats(abilityLevel)).GetStatsDescription();
                }
                else
                {
                    statsDescription = nextStats.GetStatsDescription();
                }

                upgradeOptions.Add(new(abilityType, abilityLevel, statsDescription, abilityConfig.Icon, abilityConfig.Name));
            }

            if (upgradeOptions.Count == Constants.Zero)
            {
                //Наградить

                return;
            }

            _timeService.Pause();
            _levelUpWindow.Show(upgradeOptions, level);
        }

        private List<AbilityType> GetPossibleUpgrades()
        {
            List<AbilityType> possibleUpgrades = Constants.GetEnums<AbilityType>().Except(_abilityContainer.MaxedAbilities).ToList();

            IEnumerable<AbilityType> maxedAbilities = _abilityContainer.MaxedAbilities;
            //Debug.Log("замкшено");
            Debug.Log(maxedAbilities.Count());
            //foreach (var item in maxedAbilities)
            //{
            //    Debug.Log(item.ToString());
            //}
            //Debug.Log("анлоки");
            //foreach (var item in _abilityUnlockLevel)
            //{
            //    Debug.Log(item.Key.ToString() + item.Value);
            //}

            for (int i = Constants.Zero; i < possibleUpgrades.Count; i++)
            {
                AbilityType type = possibleUpgrades[i];

                if (_abilityUnlockLevel[type] <= _abilityContainer.GetAbilityLevel(type))
                {
                    possibleUpgrades.Remove(type);
                }
            }

            return possibleUpgrades;
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

            _timeService.Unpause();
        }
    }
}
