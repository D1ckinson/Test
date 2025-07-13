using Assets.Scripts.Configs;
using Assets.Scripts.State_Machine;
using Assets.Scripts.Tools;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Factories
{
    public class EnemyFactory
    {
        private readonly LevelSettings _levelSettings;
        private readonly Pool<EnemyComponents> _pool;
        private readonly PlayerData _playerData;
        private readonly LootFactory _lootFactory;

        public EnemyFactory(LevelSettings levelSettings, PlayerData playerData, LootFactory lootFactory)
        {
            _levelSettings = levelSettings.ThrowIfNull();
            _playerData = playerData.ThrowIfNull();
            _lootFactory = lootFactory.ThrowIfNull();
            _pool = new(Create);
        }

        public int ReleaseCount => _pool.ReleaseCount;

        public EnemyComponents Spawn()
        {
            EnemyComponents enemy = _pool.Get();
            SetEnemyTransform(enemy.transform);

            return enemy;
        }

        private EnemyComponents Create()
        {
            CharacterConfig config = _levelSettings.EnemiesConfigs.First();

            Transform enemy = Object.Instantiate(config.Prefab);
            enemy.TryGetComponent(out EnemyComponents enemyComponents).ThrowIfFalse();

            enemyComponents.CharacterMovement.Initialize(config.MoveSpeed, config.RotationSpeed);
            enemyComponents.Health.Initialize(config.MaxHealth, config.InvincibilityDuration);
            enemyComponents.CollisionDamage.Initialize(config.Damage, config.DamageLayer);
            enemyComponents.DeathTriger.Initialize(enemyComponents.Health, _lootFactory, config);

            return enemyComponents;
        }

        private void SetEnemyTransform(Transform enemy)
        {
            Vector3 position = GenerateRandomPoint();
            Vector3 direction = _playerData.HeroTransform.position - position;
            direction.y = Constants.Zero;
            Quaternion rotation = Quaternion.LookRotation(direction);

            enemy.transform.SetPositionAndRotation(position, rotation);
        }

        private Vector3 GenerateRandomPoint()
        {
            float randomAngle = Random.Range(Constants.Zero, Constants.FullCircleDegrees) * Mathf.Deg2Rad;

            Vector3 direction = new(Mathf.Cos(randomAngle), Constants.Zero, Mathf.Sin(randomAngle));
            Vector3 distance = direction * _levelSettings.EnemySpawnRadius;
            Vector3 point = _playerData.HeroTransform.position + distance;
            point.y = 0.5f;//
            if (IsPositionInGameArea(point))
            {
                return point;
            }

            return _playerData.HeroTransform.position - distance;
        }


        private bool IsPositionInGameArea(Vector3 position)
        {
            float sqrDistance = (_levelSettings.GameAreaCenter - position).sqrMagnitude;

            return sqrDistance <= Mathf.Sqrt(_levelSettings.GameAreaRadius);
        }
    }
}
