using Assets.Code.Tools;
using System;

namespace Assets.Scripts
{
    [Serializable]
    public class HeroLevel
    {
        private const float TransferValue = 10f;

        private readonly Func<int, int> _experienceFormula;

        private float _buffer;
        private float _lootPercent = 1;
        private float _levelUpValue;

        public int Level { get; private set; } = 1;
        public float Value { get; private set; } = 0;

        public HeroLevel(Func<int, int> experienceFormula)
        {
            _experienceFormula = experienceFormula.ThrowIfNull();
            _levelUpValue = _experienceFormula.Invoke(Level);
        }

        public event Action<int> LevelRaised;

        public void Add(int value)
        {
            _buffer += value.ThrowIfNegative() * _lootPercent;
        }

        public void Reset()
        {
            _buffer = Constants.Zero;
            Level = Constants.One;
            Value = Constants.Zero;
            _levelUpValue = _experienceFormula.Invoke(Level);
        }

        public void SetLootPercent(int percent)
        {
            _lootPercent = Constants.One + Constants.PercentToMultiplier(percent);
        }

        public void Transfer()
        {
            if (_buffer < TransferValue)
            {
                return;
            }

            _buffer -= TransferValue;
            Value += TransferValue;

            if (Value >= _levelUpValue)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            Value -= _levelUpValue;
            _levelUpValue = _experienceFormula.Invoke(Level);

            Level++;
            LevelRaised?.Invoke(Level);
        }
    }
}
