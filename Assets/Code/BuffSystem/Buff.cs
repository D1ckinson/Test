using Assets.Code.AbilitySystem;
using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Code.Tools;
using Assets.Scripts;
using Assets.Scripts.Movement;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.AmplificationSystem
{
    public class BuffContainer
    {
        private readonly Dictionary<BuffType, Buff> _buffs = new();

        public IEnumerable<BuffType> MaxedBuffs => _buffs.Values.Where(buff => buff.IsMaxed).Select(buff => buff.Type);

        public void Add(Buff buff)
        {
            _buffs.Add(buff.ThrowIfNull().Type, buff);
        }

        public void LevelUpBuff(BuffType type)
        {
            _buffs.GetValueOrThrow(type).LevelUp();
        }

        public int GetAbilityLevel(BuffType type)
        {
            return _buffs.GetValueOrThrow(type.ThrowIfDefault()).Level;
        }

        public bool HasAbility(BuffType type)
        {
            return _buffs.ContainsKey(type.ThrowIfDefault());
        }

        internal void Upgrade(BuffType type)
        {
            throw new NotImplementedException();
        }

        internal bool HasUpgrade(BuffType type)
        {
            throw new NotImplementedException();
        }

        internal bool IsMaxed(BuffType type)
        {
            throw new NotImplementedException();
        }
    }

    public class BuffFactory
    {
        private readonly Dictionary<BuffType, BuffConfig> _configs;
        private readonly Dictionary<BuffType, Func<Buff>> _createFunc;

        private readonly HeroComponents _heroComponents;
        private readonly PlayerData _playerData;

        public BuffFactory(Dictionary<BuffType, BuffConfig> configs, HeroComponents heroComponents, PlayerData playerData)
        {
            _configs = configs.ThrowIfCollectionNullOrEmpty();
            _heroComponents = heroComponents.ThrowIfNull();
            _playerData = playerData.ThrowIfNull();

            _createFunc = new()
            {
                [BuffType.Health] = CreateHealthBuff,
                [BuffType.Regeneration] = CreateRegenBuff,
                [BuffType.Resist] = CreateResistBuff,
                [BuffType.Damage] = CreateDamageBuff,
                [BuffType.Cooldown] = CreateCooldownBuff,
                [BuffType.LootAttraction] = CreateLootAttractionBuff,
                [BuffType.Speed] = CreateSpeedBuff,
                [BuffType.Gold] = CreateGoldBuff,
                [BuffType.Experience] = CreateExperienceBuff
            };
        }

        public BuffConfig GetConfig(BuffType buffType)
        {
            return _configs.GetValueOrThrow(buffType);
        }

        public Buff Create(BuffType type)
        {
            return _createFunc.GetValueOrThrow(type).Invoke();
        }

        private HealthBuff CreateHealthBuff()
        {
            return new(_heroComponents.Health, _configs[BuffType.Health]);
        }

        private RegenBuff CreateRegenBuff()
        {
            return new(_heroComponents.Health, _configs[BuffType.Regeneration]);
        }

        private ResistBuff CreateResistBuff()
        {
            return new(_heroComponents.Health, _configs[BuffType.Resist]);
        }

        private DamageBuff CreateDamageBuff()
        {
            return new(_configs[BuffType.Damage]);
        }

        private CooldownBuff CreateCooldownBuff()
        {
            return new(_configs[BuffType.Cooldown]);
        }

        private SpeedBuff CreateSpeedBuff()
        {
            return new(_heroComponents.CharacterMovement, _configs[BuffType.Speed]);
        }

        private LootAttractionBuff CreateLootAttractionBuff()
        {
            return new(_heroComponents.LootCollector, _configs[BuffType.LootAttraction]);
        }

        private GoldBuff CreateGoldBuff()
        {
            return new(_playerData.Wallet, _configs[BuffType.Gold]);
        }

        private ExperienceBuff CreateExperienceBuff()
        {
            return new(_playerData.HeroExperience, _configs[BuffType.Experience]);
        }
    }

    public abstract class Buff
    {
        public readonly BuffType Type;

        private readonly Dictionary<int, int> _valueOnLevel;

        public Buff(BuffConfig config)
        {
            Type = config.ThrowIfDefault().Type;
        }

        public int Level { get; private set; } = 1;
        public bool IsMaxed => Level == _valueOnLevel.Count;
        protected int Value => _valueOnLevel[Level - Constants.One];

        public void LevelUp()
        {
            IsMaxed.ThrowIfTrue(new IndexOutOfRangeException());

            Level++;
            Apply();
        }

        public abstract Buff Apply();
    }

    public class HealthBuff : Buff
    {
        private readonly Health _health;

        public HealthBuff(Health health, BuffConfig config) : base(config)
        {
            _health = health.ThrowIfNull();
        }

        public override Buff Apply()
        {
            _health.SetAdditionalValue(Value);

            return this;
        }
    }

    public class RegenBuff : Buff
    {
        private readonly Health _health;

        public RegenBuff(Health health, BuffConfig config) : base(config)
        {
            _health = health.ThrowIfNull();
        }

        public override Buff Apply()
        {
            _health.SetRegeneration(Value);

            return this;
        }
    }

    public class ResistBuff : Buff
    {
        private readonly Health _health;

        public ResistBuff(Health health, BuffConfig config) : base(config)
        {
            _health = health.ThrowIfNull();
        }

        public override Buff Apply()
        {
            _health.SetResistPercent(Value);

            return this;
        }
    }

    public class DamageBuff : Buff
    {
        private readonly List<Ability> _abilities;

        public DamageBuff(BuffConfig config) : base(config)
        {
            _abilities = new();
        }

        public void Add(Ability ability)
        {
            _abilities.Add(ability.ThrowIfNull());
            ability.SetCooldownPercent(Value);
        }

        public override Buff Apply()
        {
            _abilities.ForEach(ability => ability.SetAdditionalDamage(Value));

            return this;
        }
    }

    public class CooldownBuff : Buff
    {
        private readonly List<Ability> _abilities;

        public CooldownBuff(BuffConfig config) : base(config)
        {
            _abilities = new();
        }

        public void Add(Ability ability)
        {
            _abilities.Add(ability.ThrowIfNull());
            ability.SetCooldownPercent(Value);
        }

        public override Buff Apply()
        {
            _abilities.ForEach(ability => ability.SetCooldownPercent(Value));

            return this;
        }
    }

    public class SpeedBuff : Buff
    {
        private readonly CharacterMovement _characterMovement;

        public SpeedBuff(CharacterMovement characterMovement, BuffConfig config) : base(config)
        {
            _characterMovement = characterMovement.ThrowIfNull();
        }

        public override Buff Apply()
        {
            _characterMovement.AddMaxSpeed(Value);

            return this;
        }
    }

    public class GoldBuff : Buff
    {
        private readonly Wallet _wallet;

        public GoldBuff(Wallet wallet, BuffConfig config) : base(config)
        {
            _wallet = wallet.ThrowIfNull();
        }

        public override Buff Apply()
        {
            _wallet.SetLootPercent(Value);

            return this;
        }
    }

    public class ExperienceBuff : Buff
    {
        private readonly HeroExperience _heroExperience;

        public ExperienceBuff(HeroExperience heroExperience, BuffConfig config) : base(config)
        {
            _heroExperience = heroExperience.ThrowIfNull();
        }

        public override Buff Apply()
        {
            _heroExperience.SetLootPercent(Value);

            return this;
        }
    }

    public class LootAttractionBuff : Buff
    {
        private readonly LootCollector _lootCollector;

        public LootAttractionBuff(LootCollector lootCollector, BuffConfig config) : base(config)
        {
            _lootCollector = lootCollector.ThrowIfNull();
        }

        public override Buff Apply()
        {
            _lootCollector.AddAttractionRadius(Value);

            return this;
        }
    }

    [Serializable]
    public struct BuffConfig : IUpgradeOptionUIData
    {
        [SerializeField] private List<int> _values;

        [field: SerializeField] public BuffType Type { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        public readonly Dictionary<int, int> GetStats()
        {
            Dictionary<int, int> stats = new(_values.Count);

            for (int i = Constants.Zero; i < _values.Count; i++)
            {
                stats.Add(i, _values[i]);
            }

            return stats;
        }
    }

    public enum BuffType
    {
        Default,
        Health,
        Regeneration,
        Damage,
        Cooldown,
        Speed,
        Gold,
        LootAttraction,
        Resist,
        Experience
    }
}
