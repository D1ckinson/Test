using Assets.Code.AmplificationSystem;
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
        private readonly Dictionary<Type, IContainer> _containers;

        public Trigger(IEnumerable<IContainer> containers, IEnumerable<IUpgradeFactory> factories)
        {
            _containers = containers.ToDictionary(container => container.EnumType);
            //_fac
        }

        public void OnTriger()
        {
            List<Enum> upgrades = new();

            foreach (Type upgradeType in _containers.Keys)
            {
                Enum[] enums = Constants.GetEnums(upgradeType);
                IEnumerable<Enum> collection = enums.Except(_containers[upgradeType].Maxed);

                upgrades.AddRange(collection);
            }

            List<UpgradeOption1> upgradeOption1s = new();

            for (int i = 0; i < 3; i++)
            {
                int index = Random.Range(Constants.Zero, upgrades.Count);

                Enum type = upgrades[index];
                upgrades.RemoveAt(index);
                IContainer container = _containers[type.GetType()];

                UpgradeOption1 option1 = new()
                {
                    Type = type,
                    NextLevel = container.GetAbilityLevel(type) + 1,
                    UiInfo = container.GetUpgradeInfo(type)
                };
                option1.NextLevel = container.GetAbilityLevel(type) + 1;
                option1.UiInfo = container.GetUpgradeInfo(type);

                upgradeOption1s.Add(option1);
            }
        }

        public void Upgrade(Enum @enum)
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

        private void UpgradeBuff(BuffType @enum)
        {
            _containers[@enum.GetType()].Upgrade(@enum);
        }

        private void UpgradeAbility(AbilityType @enum)
        {
            throw new NotImplementedException();
        }
    }

    public interface IUpgradeFactory
    {
    }

    public struct UpgradeOption1
    {
        public Enum Type;
        public int NextLevel;
        public UiUpgradeInfo UiInfo;
    }


    public class Container<TAbility, TEnum> : IContainer where TEnum : Enum where TAbility : IAbility<TEnum>
    {
        private readonly Dictionary<TEnum, TAbility> _abilities;

        public Container()
        {
            _abilities = new();
        }

        public IEnumerable<Enum> Maxed => _abilities.Values.Where(ability => ability.Maxed).Select(ability => ability.Type);

        public Type EnumType => typeof(TEnum);

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

        public void Upgrade(Enum @enum)
        {
            throw new NotImplementedException();
        }
    }

    public struct UiUpgradeInfo
    {
        public List<string> Text;
        public Sprite Icon;
    }

    public interface IContainer
    {
        public Type EnumType { get; }

        public IEnumerable<Enum> Maxed { get; }

        public int GetAbilityLevel(Enum type);

        public UiUpgradeInfo GetUpgradeInfo(Enum type);
        void Upgrade(Enum @enum);
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

    public class AbilityBase : IAbility<Type1>
    {
        private Config _config;

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

    public interface IAbility<T> where T : Enum
    {
        public bool Maxed { get; }
        public Enum Type { get; }
        public int Level { get; }

        public UiUpgradeInfo GetUiUpgradeInfo();
    }

    public class Config
    {
        private List<AbilityStats> _stats;

        public int MaxLevel;
        public Enum Type { get; private set; }
        public Sprite Icon { get; internal set; }

        public List<string> GetStatsDifference(int fromIndex, int toIndex)
        {
            AbilityStats fromStat = _stats[fromIndex.ThrowIfMoreThan(_stats.GetLastIndex())];
            AbilityStats toStat = _stats[toIndex.ThrowIfMoreThan(_stats.GetLastIndex())];
            AbilityStats result = fromStat - toStat;
            List<string> resultt = fromStat.GetStatsDifference(toStat);

            return resultt;
        }
    }
}
