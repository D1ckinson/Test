using Assets.Code.Data.Interfaces;
using Assets.Scripts.Tools;
using System;

namespace Assets.Scripts
{
    [Serializable]
    public class Wallet : IValueContainer
    {
        private float _lootPercent = 1;

        public float CoinsQuantity { get; private set; } = 0;

        public void Add(int value)
        {
            CoinsQuantity += value.ThrowIfZeroOrLess() * _lootPercent;
        }

        public void SetLootPercent(float percent)
        {
            _lootPercent = Constants.PercentToMultiplier(percent.ThrowIfNegative());
        }
    }
}
