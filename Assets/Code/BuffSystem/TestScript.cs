using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.AbilitySystem
{
    internal class TestScript { }

    public class Trigger
    {
        private readonly Dictionary<Enum, IContainer> _containers;

        public Trigger(IEnumerable<IContainer> containers)
        {
            _containers = containers.ToDictionary(container => container.Type);
        }

        public void OnTriger()
        {
            List<Enum> upgrades = new();

            foreach (Enum upgradeType in _containers.Keys)
            {
                Enum[] enums = new Enum[1];/* Constants.GetEnums(upgradeType.GetType());*/
                IEnumerable<Enum> collection = enums.Except(_containers[upgradeType].Maxed);

                upgrades.AddRange(collection);
            }

            List<Enum> chosen = new();

            for (int i = 0; i < 3; i++)
            {
                int index = Random.Range(Constants.Zero, upgrades.Count);

                chosen.Add(upgrades[index]);
                upgrades.RemoveAt(index);
            }

            List<UpgradeOption> upgradeOption1s = new();

            for (int i = 0; i < chosen.GetLastIndex(); i++)
            {
                Enum type = chosen[i];
                //UpgradeOption option1 = new(type);
                IContainer container = _containers[type];
                //option1.NextLevel = container.GetAbilityLevel(type) + 1;
                //option1.UiInfo = container.GetUpgradeInfo(type);

                //upgradeOption1s.Add(option1);
            }
        }
    }

    public class Container<TAbility, TEnum> : IContainer where TEnum : Enum where TAbility : IAbility
    {
        private readonly Dictionary<TEnum, TAbility> _abilities;

        public Container(Dictionary<TEnum, TAbility> abilities)
        {
            _abilities = abilities;

        }

        public IEnumerable<Enum> Maxed => _abilities.Values.Where(ability => ability.Maxed).Select(ability => ability.Type);

        public Enum Type => throw new NotImplementedException();

        public int GetAbilityLevel(Enum type)
        {
            _abilities.TryGetValue((TEnum)type, out TAbility ability);//

            return ability == null ? 0 : ability.Level;
        }

        public UiUpgradeInfo GetUpgradeInfo(Enum type)
        {
            _abilities.TryGetValue((TEnum)type, out TAbility ability);//
            UiUpgradeInfo info = ability.GetUiUpgradeInfo();

            return info;
        }
    }

    public struct UiUpgradeInfo
    {
        public List<string> Text;
        public Sprite Icon;
    }

    public interface IContainer
    {
        public Enum Type { get; }

        public IEnumerable<Enum> Maxed { get; }

        public int GetAbilityLevel(Enum type);

        public UiUpgradeInfo GetUpgradeInfo(Enum type);
    }

    public enum Type1
    {
        one,
        two
    }

    public enum Type2
    {
        one,
        two
    }

    public class AbilityBase : IAbility
    {
        private readonly Config _config;

        public int Level { get; private set; }
        public bool Maxed => Level == _config.MaxLevel;
        public Enum Type => _config.Type;

        public UiUpgradeInfo GetUiUpgradeInfo()
        {
            UiUpgradeInfo info = new()
            {
                Icon = _config.Icon,
                Text = _config.GetStatsDifference(Level - 1, Level)
            };

            return info;
        }
    }

    public class Ability1 : AbilityBase
    {
    }

    public class Ability2 : AbilityBase
    {
    }

    public interface IAbility
    {
        public bool Maxed { get; }
        public Enum Type { get; }
        public int Level { get; }

        public UiUpgradeInfo GetUiUpgradeInfo();
    }

    public class Config
    {
        private readonly List<AbilityStats> _stats;

        public int MaxLevel;
        public Enum Type { get; private set; }
        public Sprite Icon { get; internal set; }

        public List<string> GetStatsDifference(int fromIndex, int toIndex)
        {
            AbilityStats fromStat = _stats[fromIndex.ThrowIfMoreThan(_stats.GetLastIndex())];
            AbilityStats toStat = _stats[toIndex.ThrowIfMoreThan(_stats.GetLastIndex())];
            List<string> result = new();/* fromStat.GetStatsDescription(toStat);*/

            return result;
        }
    }
}