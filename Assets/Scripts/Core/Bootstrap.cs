using UnityEngine;
using Assets.Scripts.Spawners;
using Assets.Scripts.Tools;
using Assets.Scripts.Economy;
using Assets.Scripts.Ui;
using Assets.Scripts.ExperienceSystem;

namespace Assets.Scripts.Core
{
    internal class Bootstrap : MonoBehaviour
    {
        [SerializeField] private HeroSpawner _heroSpawner;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private LevelUpWindow _levelUpWindow;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _heroSpawner.ThrowIfNull();
            _enemySpawner.ThrowIfNull();
            _wallet.ThrowIfNull();
            _levelUpWindow.ThrowIfNull();
        }
#endif

        private void Awake()
        {
            Transform heroTransform = _heroSpawner.Spawn();

            if (heroTransform.TryGetComponent(out CoinCollector coinCollector) == false)
            {
                throw new MissingComponentException();
            }

            _wallet.Initialize(coinCollector);
            _enemySpawner.Initialize(heroTransform);
            _enemySpawner.Run();

            if (heroTransform.TryGetComponent(out HeroLevel heroLevel))
            {
                throw new MissingComponentException();
            }

            _levelUpWindow.Initialize(heroLevel);
        }
    }
}
