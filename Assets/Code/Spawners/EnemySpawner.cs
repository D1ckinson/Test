using Assets.Code.Data;
using Assets.Code.Tools;
using Assets.Scripts;
using Assets.Scripts.Factories;
using Assets.Scripts.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Spawners
{
    public class EnemySpawner
    {
        private readonly EnemyFactory _enemyFactory;
        private readonly GameTimer _gameTimer;
        private readonly Dictionary<int, CharacterType> _spawnTypeByTime;

        private CharacterType _spawnType;
        private float _delay;
        private int _changeTypeTimeThreshold;
        private bool _canChangeSpawnType = true;

        public EnemySpawner(EnemyFactory enemyFactory, Dictionary<int, CharacterType> spawnTypeByTime)
        {
            _enemyFactory = enemyFactory.ThrowIfNull();
            _spawnTypeByTime = spawnTypeByTime.ThrowIfNullOrEmpty();

            _changeTypeTimeThreshold = _spawnTypeByTime.Keys.First();
            _spawnType = _spawnTypeByTime.Values.ElementAt(Constants.One);

            _gameTimer = new();
        }

        public void Update()
        {
            _gameTimer.Update();

            if (_changeTypeTimeThreshold < _gameTimer.PassedTime && _canChangeSpawnType)
            {
                ChangeSpawnType();
            }

            SpawnEnemy();
        }

        public void Reset()
        {
            _gameTimer.Reset();
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

            _enemyFactory.Spawn(_spawnType);
            _delay = Constants.Zero;
        }


        private void ChangeSpawnType()
        {
            int? nextThreshold = _spawnTypeByTime.Keys.SkipWhile(time => time <= _changeTypeTimeThreshold).FirstOrDefault();

            if (nextThreshold.IsNull())
            {
                _canChangeSpawnType = false;

                return;
            }

            _spawnType = _spawnTypeByTime[(int)nextThreshold];
            _changeTypeTimeThreshold = (int)nextThreshold;
        }
    }
}
