using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.Economy
{
    public class Wallet : MonoBehaviour
    {
        public int CoinsQuantity { get; private set; } = 0;

        public void Initialize(CoinCollector coinCollector)
        {
            coinCollector.ThrowIfNull();

            coinCollector.Collect += AddCoin;
        }

        private void AddCoin(Coin coin)
        {
            CoinsQuantity++;

            string message = $"Собранно: {CoinsQuantity} монет";
            Debug.Log(message);
        }
    }
}
