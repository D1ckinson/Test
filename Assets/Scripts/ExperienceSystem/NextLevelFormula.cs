using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts.ExperienceSystem
{
    [CreateAssetMenu(fileName ="Level Up Formula",menuName ="Game/LevelUpFormula")]
    public class NextLevelFormula : ScriptableObject
    {
        [field: SerializeField][field: Min(1)] public int FixedExperience { get; private set; } = 1;
        [field: SerializeField][field: Min(1)] public int ExperienceCoefficient { get; private set; } = 1;
        [field: SerializeField][field: Min(1)] public float Degree { get; private set; } = 1;

        public int CalculateNextLevel(int level)
        {
            level.ThrowIfZeroOrLess();

            return (int)(FixedExperience * level + ExperienceCoefficient * MathF.Pow(level, Degree));
        }
    }
}