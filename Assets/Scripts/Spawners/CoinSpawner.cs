using Assets.Scripts.Economy;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.Spawners
{
    internal class CoinSpawner : MonoBehaviour
    {
        [SerializeField] private Coin _coinPrefab;
        [SerializeField][Min(0.1f)] private float _maxSpawnOffset = 1f;

        private Pool<Coin> _pool;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _coinPrefab.ThrowIfNull();
        }
#endif

        private void Awake()
        {
            _pool = new(CreateFunc);
        }

        public void Spawn(Vector3 position, int count)
        {
            count.ThrowIfZeroOrLess();

            Vector3 spawnPosition = position + GenerateOffset();
            Quaternion spawnRotation = GenerateRotation();

            Coin coin = _pool.Get();
            coin.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
        }

        private Coin CreateFunc()
        {
            Coin coin = Instantiate(_coinPrefab);
            coin.Collected += OnCoinCollected;

            return coin;
        }

        private void OnCoinCollected(Coin coin)
        {
            coin.ThrowIfNull();
            _pool.Return(coin);
        }

        private Vector3 GenerateOffset()
        {
            Vector3 offset = new()
            {
                x = Random.Range(Constants.Zero, _maxSpawnOffset),
                z = Random.Range(Constants.Zero, _maxSpawnOffset)
            };

            return offset;
        }

        private Quaternion GenerateRotation()
        {
            Quaternion rotation = new()
            {
                y = Random.Range(Constants.Zero, Constants.FullCircleDegrees),
            };

            return rotation;
        }
    }
}
