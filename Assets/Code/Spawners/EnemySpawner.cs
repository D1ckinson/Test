using Assets.Code.Data.Setting_Structures;
using Assets.Code.Tools;
using Assets.Scripts.Factories;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Code.Spawners
{
    public class EnemySpawner
    {
        private readonly EnemyFactory _enemyFactory;
        private readonly Timer _timer;
        private readonly SpawnTypeByTime[] _spawnTypeByTime;

        private SpawnTypeByTime _spawnType;
        private float _delay;

        public EnemySpawner(EnemyFactory enemyFactory, SpawnTypeByTime[] spawnTypeByTime)
        {
            _enemyFactory = enemyFactory.ThrowIfNull();
            _spawnTypeByTime = spawnTypeByTime.ThrowIfNullOrEmpty();

            _timer = new();
        }

        public void Run()
        {
            _spawnType = _spawnTypeByTime[Constants.Zero];
            SetSpawnType();
            UpdateService.Register(SpawnEnemy);
        }

        public void Pause()
        {
            _timer.Pause();
            UpdateService.Unregister(SpawnEnemy);
        }

        public void Reset()
        {
            UpdateService.Unregister(SpawnEnemy);
            _timer.Pause();
            _timer.Completed -= SetSpawnType;
            _enemyFactory.DisableAll();
        }

        private void SpawnEnemy()
        {
            if (_enemyFactory.IsSpawnLimitReached)
            {
                return;
            }

            _delay += Time.deltaTime;

            if (_delay < _enemyFactory.Delay)
            {
                return;
            }

            _enemyFactory.Spawn(_spawnType.Type);
            _delay = Constants.Zero;
        }

        private void SetSpawnType()
        {
            _timer.Completed -= SetSpawnType;
            int nextIndex = _spawnTypeByTime.IndexOf(_spawnType) + Constants.One;

            if (nextIndex == Constants.Zero)
            {
                return;
            }

            int time = _spawnTypeByTime[nextIndex].Time;
            _timer.Start(time);
            _timer.Completed += SetSpawnType;
        }
    }
}
