using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts.ExperienceSystem
{
    [RequireComponent(typeof(ExperienceCollector))]
    internal class HeroLevel : MonoBehaviour
    {
        [SerializeField] private NextLevelFormula _nextLevelFormula;

        private float _currentExperience;
        private float _experienceForLevelUp;
        private int _level = Constants.One;

        internal event Action<int> LevelUp;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _nextLevelFormula.ThrowIfNull();
        }
#endif

        private void Awake()
        {
            _experienceForLevelUp = _nextLevelFormula.CalculateNextLevel(_level);
        }

        internal void CollectExperience(int value)
        {
            value.ThrowIfZeroOrLess();
            _currentExperience += value;

            TryLevelUp();
        }

        private void TryLevelUp()
        {
            if (_currentExperience < _experienceForLevelUp)
            {
                return;
            }

            _level++;
            LevelUp?.Invoke(_level);

            _currentExperience -= _experienceForLevelUp;
            _experienceForLevelUp = _nextLevelFormula.CalculateNextLevel(_level);
        }
    }
}
