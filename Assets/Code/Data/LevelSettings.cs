using Assets.Code;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Game/LevelSettings")]
    public class LevelSettings : ScriptableObject
    {
        [field: Header("Characters Configs")]
        [field: SerializeField] public List<CharacterConfig> EnemiesConfigs { get; private set; }
        [field: SerializeField] public CharacterConfig HeroConfig { get; private set; }

        [field: Header("Loot List")]
        [field: SerializeField] public List<Loot> Loots { get; private set; }

        [field: Header("Enemy Spawn Settings")]
        [field: SerializeField][field: Min(1)] public float EnemySpawnRadius { get; private set; } = 1f;
        [field: SerializeField][field: Min(0.1f)] public float EnemySpawnDelay { get; private set; } = 0.6f;
        [field: SerializeField][field: Min(1)] public int MaxEnemyCount { get; private set; } = 50;

        [field: Header("Game Area Settings")]
        [field: SerializeField][field: Min(1)] public float GameAreaRadius { get; private set; } = 1f;
        [field: SerializeField] public Vector3 GameAreaCenter { get; private set; } = Vector3.zero;

        [field: Header("Abilities")]
        [field: SerializeField] public List<NewAbilityConfig> AbilitiesConfigs { get; private set; }

        [Header("Level Formula Settings")]
        [SerializeField][Min(1)] private int _fixedExperience = 1;
        [SerializeField][Min(1)] private int _experienceCoefficient = 1;
        [SerializeField][Min(1)] private float _degree = 1;

        public int CalculateNextLevelExperience(int level)
        {
            level.ThrowIfZeroOrLess();

            return (int)(_fixedExperience * level + _experienceCoefficient * MathF.Pow(level, _degree));
        }
    }
}
