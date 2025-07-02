using Assets.Characters;
using Assets.Scripts.Tools;
using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts.Combat;
using Assets.Scripts.Spawners.Configs;

namespace Assets.Scripts.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        private const int MaxSpawnChance = 100;

        [SerializeField] private EnemySpawnerConfig _config;
        [SerializeField] private SphereCollider _gameArea;
        [SerializeField] private ExperienceSpawner _experienceSpawner;
        [SerializeField] private CoinSpawner _coinSpawner;

        private Transform _hero;
        private Coroutine _spawnProcess;
        private Health _heroHealth;
        private Pool<Enemy> _pool;
        private bool _isInitialized = false;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _config.ThrowIfNull();
            _gameArea.ThrowIfNull();
            _experienceSpawner.ThrowIfNull();
        }

        private void OnDrawGizmos()
        {
            if (_config != null && _hero != null)
            {
                CustomGizmos.DrawCircle(_hero.transform.position, _config.Radius);
            }

            if (_gameArea != null)
            {
                CustomGizmos.DrawCircle(_gameArea.transform.position, _gameArea.radius, Color.blue);
            }
        }
#endif

        private void OnDestroy()
        {
            if (_hero != null)
            {
                _heroHealth.Died -= Stop;
            }
        }

        public void Initialize(Transform hero)
        {
            hero.ThrowIfNull();
            _hero = hero;

            if (hero.TryGetComponent(out Health heroHealth) == false)
            {
                throw new MissingComponentException();
            }

            _heroHealth = heroHealth;
            _heroHealth.Died += Stop;

            _pool = new(CreateFunc);

            _isInitialized = true;
        }

        public void Run()
        {
            if (_isInitialized == false)
            {
                throw new InvalidOperationException();
            }

            _spawnProcess ??= StartCoroutine(SpawnProcess());
        }

        private void Stop()
        {
            if (_spawnProcess == null)
            {
                return;
            }

            StopCoroutine(_spawnProcess);
            _spawnProcess = null;
        }

        private IEnumerator SpawnProcess()
        {
            WaitForSeconds wait = new(_config.SpawnDelay);

            while (true)
            {
                if (_pool.ReleaseCount < _config.MaxCount)
                {
                    Enemy enemy = _pool.Get();
                    SetEnemyTransform(enemy);
                }

                yield return wait;
            }
        }

        private void SetEnemyTransform(Enemy enemy)
        {
            Vector3 position = GenerateRandomPoint();
            Vector3 direction = _hero.position - position;
            direction.y = Constants.Zero;
            Quaternion rotation = Quaternion.LookRotation(direction);

            enemy.transform.SetPositionAndRotation(position, rotation);
        }

        private Enemy CreateFunc()
        {
            Enemy enemy = Instantiate(_config.Prefab);
            enemy.Initialize(_hero);
            enemy.Died += OnEnemyDeath;

            return enemy;
        }

        private void OnEnemyDeath(Enemy enemy)
        {
            _pool.Return(enemy);
            Vector3 position = enemy.transform.position;

            _experienceSpawner.Spawn(position, enemy.ExperienceValue);

            if (UnityEngine.Random.Range(Constants.Zero, MaxSpawnChance) > enemy.CoinDropChance)
            {
                return;
            }

            _coinSpawner.Spawn(position, enemy.CoinValue);
        }

        private Vector3 GenerateRandomPoint()
        {
            float randomAngle = UnityEngine.Random.Range(Constants.Zero, Constants.FullCircleDegrees) * Mathf.Deg2Rad;

            Vector3 direction = new(Mathf.Cos(randomAngle), Constants.Zero, Mathf.Sin(randomAngle));
            Vector3 distance = direction * _config.Radius;
            Vector3 point = _hero.position + distance;
            point.y = 0.5f;//
            if (_gameArea.ClosestPoint(point) == point)
            {
                return point;
            }

            return _hero.position - distance;
        }
    }
}
