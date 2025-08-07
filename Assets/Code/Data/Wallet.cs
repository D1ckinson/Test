using Assets.Code.Data.Interfaces;
using Assets.Scripts.Tools;
using System;

namespace Assets.Scripts
{
    [Serializable]
    public class Wallet : IValueContainer
    {
        private float _lootMultiplier = 1;

        public float CoinsQuantity { get; private set; } = 0;

        public void Add(int value)
        {
            CoinsQuantity += value.ThrowIfZeroOrLess() * _lootMultiplier;
        }

        public void Spend(float value)
        {
            CoinsQuantity -= value.ThrowIfNegative().ThrowIfMoreThan(CoinsQuantity + Constants.One);
        }

        public void SetLootPercent(int percent)
        {
            _lootMultiplier = Constants.PercentToMultiplier(percent.ThrowIfNegative());
        }
    }
}
