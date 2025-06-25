using UnityEngine;
using Assets.Scripts.Spawners;
using Assets.Scripts.Tools;

namespace Assets.Scripts.Core
{
    internal class Bootstrap : MonoBehaviour
    {
        [SerializeField] private HeroSpawner _heroSpawner;
        [SerializeField] private EnemySpawner _enemySpawner;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _heroSpawner.ThrowIfNull();
            _enemySpawner.ThrowIfNull();
        }
#endif

        private void Awake()
        {
            Transform heroTransform = _heroSpawner.Spawn();

            _enemySpawner.Initialize(heroTransform);
            _enemySpawner.Run();
        }
    }
}
