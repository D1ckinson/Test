using Assets.Code;
using Assets.Code.Shop;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Game/LevelSettings")]
    public class LevelSettings : ScriptableObject
    {
        [field: Header("Characters Configs")]
        [field: SerializeField] private List<CharacterConfig> _enemiesConfigs;
        [field: SerializeField] public CharacterConfig HeroConfig { get; private set; }

        [field: Header("All Loot")]
        [field: SerializeField] public List<Loot> Loots { get; private set; }

        [field: Header("Enemy Spawn Settings")]
        [field: SerializeField] public EnemySpawnerSettings EnemySpawnerSettings { get; private set; }
        [field: SerializeField] private List<SpawnTypeByTime> _spawnTypeByTimes;

        [field: Header("Game Area Settings")]
        [field: SerializeField] public GameAreaSettings GameAreaSettings { get; private set; }

        [field: Header("Abilities")]
        [field: SerializeField] public UpgradeCost UpgradeCost { get; private set; }

        [field: SerializeField] private List<AbilityConfig> _abilitiesConfigs;

        [Header("Level Formula Settings")]
        [SerializeField][Min(1)] private int _fixedExperience = 100;
        [SerializeField][Min(1)] private int _experienceCoefficient = 50;
        [SerializeField][Min(1)] private float _degree = 1.3f;

        public int CalculateExperienceForNextLevel(int current)
        {
            current.ThrowIfZeroOrLess();

            return (int)(_fixedExperience * current + _experienceCoefficient * MathF.Pow(current, _degree));
        }

        public Dictionary<AbilityType, AbilityConfig> AbilityConfigs => _abilitiesConfigs.ToDictionary(config => config.Type, config => config);

        public Dictionary<CharacterType, CharacterConfig> EnemyConfigs => _enemiesConfigs.ToDictionary(config => config.Type, config => config);

        public Dictionary<int, CharacterType> SpawnTypesByTime => _spawnTypeByTimes.ToDictionary(item => item.Time, item => item.Type);

        [Serializable]
        private struct SpawnTypeByTime
        {
            [field: SerializeField] public int Time { get; private set; }
            [field: SerializeField] public CharacterType Type { get; private set; }
        }
    }
}
