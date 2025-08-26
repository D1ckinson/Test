using Assets.Code;
using Assets.Code.Data.Setting_Structures;
using Assets.Code.Shop;
using Assets.Code.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Game/LevelSettings")]
    public partial class LevelSettings : ScriptableObject
    {
        [field: Header("Characters Configs")]
        [field: SerializeField] private CharacterConfig[] _enemiesConfigs;
        [field: SerializeField] public CharacterConfig HeroConfig { get; private set; }

        [field: Header("All Loot")]
        [field: SerializeField] public Loot[] Loots { get; private set; }

        [field: Header("Enemy Spawn Settings")]
        [field: SerializeField] public EnemySpawnerSettings EnemySpawnerSettings { get; private set; }
        [field: SerializeField] public SpawnTypeByTime[] SpawnTypeByTimes { get; private set; }

        [field: Header("Game Area Settings")]
        [field: SerializeField] public GameAreaSettings GameAreaSettings { get; private set; }

        [field: Header("Abilities")]
        [field: SerializeField] public UpgradeCost UpgradeCost { get; private set; }

        [field: SerializeField] private AbilityConfig[] _abilitiesConfigs;

        [Header("Level Formula Settings")]
        [SerializeField][Min(1)] private int _fixedExperience = 100;
        [SerializeField][Min(1)] private int _experienceCoefficient = 50;
        [SerializeField][Min(1)] private float _degree = 1.3f;

        public int CalculateExperienceForNextLevel(int current)
        {
            current.ThrowIfZeroOrLess();

            return (int)(_fixedExperience * current + _experienceCoefficient * MathF.Pow(current, _degree));
        }

        public Dictionary<AbilityType, AbilityConfig> AbilityConfigs => _abilitiesConfigs.ToDictionary(config => config.Type);

        public Dictionary<CharacterType, CharacterConfig> EnemyConfigs => _enemiesConfigs.ToDictionary(config => config.Type);
    }
}
