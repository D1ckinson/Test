using Assets.Scripts.Configs;
using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts.State_Machine
{
    public class GameState : State
    {
        private readonly LevelSettings _levelSettings;
        private readonly FactoryService _factoryService;
        private readonly PlayerData _playerData;

        private Transform _hero;
        private float _enemySpawnTime;

        public GameState(StateMachine stateMachine, LevelSettings levelSettings, FactoryService factoryService, PlayerData playerData) : base(stateMachine)
        {
            _levelSettings = levelSettings.ThrowIfNull();
            _factoryService = factoryService.ThrowIfNull();
            _hero = _factoryService.HeroFactory.Create();
            _playerData = playerData.ThrowIfNull();
        }

        public override void Enter()
        {
            _hero ??= _factoryService.HeroFactory.Create();
        }

        public override void Update()
        {
            SpawnEnemy();
        }

        public override void Exit()
        {
            throw new NotImplementedException();
        }

        private void SpawnEnemy()
        {
            if (_factoryService.EnemyFactory.ReleaseCount >= _levelSettings.MaxEnemyCount)
            {
                return;
            }

            _enemySpawnTime += Time.deltaTime;

            if (_enemySpawnTime < _levelSettings.EnemySpawnDelay)
            {
                return;
            }

            EnemyComponents enemy = _factoryService.EnemyFactory.Spawn();
            enemy.DirectionTeller.SetTarget(_hero);
            _enemySpawnTime = Constants.Zero;
        }
    }
}
