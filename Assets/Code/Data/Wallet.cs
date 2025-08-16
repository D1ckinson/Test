using Assets.Code.Data.Interfaces;
using Assets.Scripts.Tools;
using System;

namespace Assets.Scripts
{
    [Serializable]
    public class Wallet : IValueContainer
    {
        private float _lootMultiplier = 1;

        public event Action<float> ValueChanged;

        public int CoinsQuantity { get; private set; } = 0;

        public void Add(int value)
        {
            CoinsQuantity += value.ThrowIfZeroOrLess();
            ValueChanged?.Invoke(CoinsQuantity);
        }

        public void Spend(int value)
        {
            CoinsQuantity -= value.ThrowIfNegative().ThrowIfMoreThan(CoinsQuantity + Constants.One);
            ValueChanged?.Invoke(CoinsQuantity);
        }

        public void SetLootPercent(int percent)
        {
            _lootMultiplier = Constants.PercentToMultiplier(percent.ThrowIfNegative());
        }
    }
}
