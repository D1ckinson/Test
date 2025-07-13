using Assets.Code.Data.Interfaces;
using Assets.Scripts.Configs;
using Assets.Scripts.Tools;
using System;

namespace Assets.Scripts
{
    [Serializable]
    public class HeroExperience : IValueContainer
    {
        public int Level { get; private set; } = 1;
        public float CurrentExperience { get; private set; } = 0;
        public float ExperienceForLevelUp { get; private set; }

        private readonly LevelSettings _levelSettings;

        public HeroExperience(LevelSettings levelSettings)
        {
            _levelSettings = levelSettings.ThrowIfNull();
            ExperienceForLevelUp = _levelSettings.CalculateNextLevelExperience(Level);
        }

        public void Add(int value)
        {
            CurrentExperience += value.ThrowIfNegative();
            TryLevelUp();
        }

        public event Action<int> LevelUp;

        public void Reset()
        {
            Level = Constants.One;
            CurrentExperience = Constants.Zero;
        }

        private void TryLevelUp()
        {
            if (CurrentExperience < ExperienceForLevelUp)
            {
                return;
            }

            Level++;
            LevelUp?.Invoke(Level);

            CurrentExperience -= ExperienceForLevelUp;
            ExperienceForLevelUp = _levelSettings.CalculateNextLevelExperience(Level);
        }

    }
}
