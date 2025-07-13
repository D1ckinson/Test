using Assets.Code.Data.Interfaces;
using Assets.Scripts.Tools;
using System;

namespace Assets.Scripts
{
    [Serializable]
    public class Wallet : IValueContainer
    {
        public int CoinsQuantity { get; private set; } = 0;

        public void Add(int value)
        {
            CoinsQuantity += value.ThrowIfZeroOrLess();
        }
    }
}
