using Assets.Scripts.Configs;
using Assets.Scripts.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Factories
{
    public class EnemyFactory
    {
        private readonly Dictionary<CharacterType, CharacterConfig> _enemiesConfigs;
        private readonly LootFactory _lootFactory;
        private readonly Transform _hero;
        private readonly EnemySpawnerSettings _spawnerSettings;
        private readonly GameAreaSettings _gameAreaSettings;
        private readonly Pool<EnemyComponents> _pool;

        public EnemyFactory(Dictionary<CharacterType, CharacterConfig> enemiesConfigs, LootFactory lootFactory, Transform hero, EnemySpawnerSettings spawnerSettings, GameAreaSettings gameAreaSettings)
        {
            _enemiesConfigs = enemiesConfigs.ThrowIfCollectionNullOrEmpty();
            _lootFactory = lootFactory.ThrowIfNull();
            _hero = hero.ThrowIfNull();

            _spawnerSettings = spawnerSettings.ThrowIfDefault();
            _gameAreaSettings = gameAreaSettings.ThrowIfDefault();

            _pool = new(Create);
        }

        public bool IsSpawnLimitReached => _pool.ReleaseCount == _spawnerSettings.SpawnLimit;
        public float Delay => _spawnerSettings.Delay;

        public EnemyComponents Spawn(CharacterType characterType)
        {
            characterType.ThrowIfNull();
            EnemyComponents enemy = _pool.Get();

            if (enemy.CharacterType != characterType)
            {
                SetStats(enemy, _enemiesConfigs[characterType]);
            }

            SetTransform(enemy.transform);

            return enemy;
        }

        private void SetStats(EnemyComponents enemy, CharacterConfig config)
        {
            enemy.CharacterMovement.SetMoveStat(config.MoveSpeed);
            enemy.Health.SetMaxValue(config.MaxHealth);
            enemy.CollisionDamage.SetDamage(config.Damage);
            enemy.DeathTriger.SetLoot(config.Loot);
            enemy.SetType(config.Type);
            //смена цвета материала
        }

        private EnemyComponents Create()
        {
            CharacterConfig config = _enemiesConfigs.Values.First();

            Transform enemy = Object.Instantiate(config.Prefab);
            enemy.TryGetComponent(out EnemyComponents enemyComponents).ThrowIfFalse();

            enemyComponents.CharacterMovement.Initialize(config.MoveSpeed, config.RotationSpeed);
            enemyComponents.Health.Initialize(config.MaxHealth, config.InvincibilityDuration);
            enemyComponents.CollisionDamage.Initialize(config.Damage, config.DamageLayer);
            enemyComponents.DeathTriger.Initialize(enemyComponents.Health, _lootFactory, config.Loot);
            enemyComponents.DirectionTeller.SetTarget(_hero);
            enemyComponents.SetType(config.Type);

            return enemyComponents;
        }

        private void SetTransform(Transform enemy)
        {
            Vector3 position = GenerateRandomPoint();
            Vector3 direction = _hero.position - position;

            direction.y = Constants.Zero;
            Quaternion rotation = Quaternion.LookRotation(direction);

            enemy.transform.SetPositionAndRotation(position, rotation);
        }

        private Vector3 GenerateRandomPoint()
        {
            float randomAngle = Random.Range(Constants.Zero, Constants.FullCircleDegrees) * Mathf.Deg2Rad;

            Vector3 direction = new(Mathf.Cos(randomAngle), Constants.Zero, Mathf.Sin(randomAngle));
            Vector3 distance = direction * _spawnerSettings.Radius;
            Vector3 point = _hero.position + distance;

            return IsPositionInGameArea(point) ? point : _hero.position - distance;
        }

        private bool IsPositionInGameArea(Vector3 position)
        {
            float sqrDistance = (_gameAreaSettings.Center - position).sqrMagnitude;

            return sqrDistance <= Mathf.Sqrt(_gameAreaSettings.Radius);
        }
    }
}
