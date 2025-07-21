using Assets.Code;
using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Code.Data;
using Assets.Scripts.Factories;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.State_Machine
{
    public class GameState : State
    {
        private readonly HeroComponents _heroComponents;
        private readonly AbilityFactory _abilityFactory;
        private readonly EnemyFactory _enemyFactory;
        private readonly GameTimer _gameTimer;
        private readonly Dictionary<int, CharacterType> _spawnTypeByTime;

        private CharacterType _enemySpawnType;
        private float _enemySpawnDelay;
        private int _enemySpawnTypeThreshold;
        private bool _canChangeSpawnType = true;

        public GameState(StateMachine stateMachine, HeroComponents heroComponents, EnemyFactory enemyFactory,
            AbilityFactory abilityFactory, GameTimer gameTimer, Dictionary<int, CharacterType> spawnTypeByTime) : base(stateMachine)
        {
            _heroComponents = heroComponents.ThrowIfNull();
            _enemyFactory = enemyFactory.ThrowIfNull();
            _abilityFactory = abilityFactory.ThrowIfNull();
            _gameTimer = gameTimer.ThrowIfNull();
            _spawnTypeByTime = spawnTypeByTime.ThrowIfCollectionNullOrEmpty();
        }

        public override void Enter()
        {
            _heroComponents.AbilityContainer.Add(_abilityFactory.Create(AbilityType.SwordStrike));
            _enemySpawnType = _spawnTypeByTime.Values.First();

            _gameTimer.Reset();
            _enemySpawnTypeThreshold = _spawnTypeByTime.Keys.ElementAt(Constants.One);
        }

        public override void Update()
        {
            _gameTimer.Update();

            //_enemySpawner.Update();
            //_спавнерУсилений.Update();

            if (_enemySpawnTypeThreshold < _gameTimer.PassedTime && _canChangeSpawnType)
            {
                ChangeSpawnType();
            }

            SpawnEnemy();
        }

        public override void Exit()
        {
            throw new NotImplementedException();
        }

        private void SpawnEnemy()
        {
            if (_enemyFactory.IsSpawnLimitReached)
            {
                return;
            }

            _enemySpawnDelay += Time.deltaTime;

            if (_enemySpawnDelay < _enemyFactory.Delay)
            {
                return;
            }

            _enemyFactory.Spawn(_enemySpawnType);
            _enemySpawnDelay = Constants.Zero;
        }


        private void ChangeSpawnType()
        {
            int? nextThreshold = _spawnTypeByTime.Keys.SkipWhile(time => time <= _enemySpawnTypeThreshold).FirstOrDefault();

            if (nextThreshold == null)
            {
                _canChangeSpawnType = false;

                return;
            }

            _enemySpawnType = _spawnTypeByTime[(int)nextThreshold];
            _enemySpawnTypeThreshold = (int)nextThreshold;
        }
    }
}
