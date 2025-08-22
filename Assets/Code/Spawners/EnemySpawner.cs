using Assets.Code.CharactersLogic.EnemyLogic;
using Assets.Code.Data.Setting_Structures;
using Assets.Code.Tools;
using Assets.Scripts.Factories;
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
            UpdateService.RegisterUpdate(SpawnEnemy);
        }

        public void Pause()
        {
            _timer.Pause();
            _enemyFactory.StopAll();
            UpdateService.UnregisterUpdate(SpawnEnemy);
        }

        public void Continue()
        {
            _timer.Continue();
            _enemyFactory.ContinueAll();
            UpdateService.RegisterUpdate(SpawnEnemy);
        }

        public void Reset()
        {
            UpdateService.UnregisterUpdate(SpawnEnemy);
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

            EnemyComponents enemy = _enemyFactory.Spawn(_spawnType.Type);
            enemy.Health.Died += OnDeath;
            _delay = Constants.Zero;

            void OnDeath()
            {
                enemy.Health.Died -= OnDeath;
                enemy.SetActive(false);
                enemy.CharacterMovement.Stop();
            }
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
