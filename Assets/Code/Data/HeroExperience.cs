using Assets.Code.Data.Interfaces;
using Assets.Scripts.Configs;
using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class HeroExperience : IValueContainer
    {
        public int Level { get; private set; } = 1;
        public float CurrentExperience { get; private set; } = 0;
        public float ExperienceForLevelUp { get; private set; }

        private readonly LevelSettings _levelSettings;
        private float _lootMultiplier = 1;

        public HeroExperience(LevelSettings levelSettings)
        {
            _levelSettings = levelSettings.ThrowIfNull();
            ExperienceForLevelUp = _levelSettings.CalculateNextLevelExperience(Level);
        }

        public void Add(int value)
        {
            CurrentExperience += value.ThrowIfNegative() * _lootMultiplier;
            TryLevelUp();
        }

        public event Action<int> LevelUp;

        public void Reset()
        {
            Level = Constants.One;
            CurrentExperience = Constants.Zero;
        }

        public void SetLootPercent(float value)
        {
            _lootMultiplier = Constants.PercentToMultiplier(value.ThrowIfNegative());
        }

        private void TryLevelUp()
        {
            if (CurrentExperience < ExperienceForLevelUp)
            {
                return;
            }

            Level++;
            LevelUp?.Invoke(Level);
            Debug.Log("Левел ап!");
            CurrentExperience -= ExperienceForLevelUp;
            ExperienceForLevelUp = _levelSettings.CalculateNextLevelExperience(Level);
        }
    }
}
