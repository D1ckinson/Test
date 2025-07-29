using Assets.Code;
using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Code.Spawners;
using Assets.Scripts.Tools;
using System;

namespace Assets.Scripts.State_Machine
{
    public class GameState : State
    {
        private readonly HeroComponents _heroComponents;
        private readonly AbilityFactory _abilityFactory;
        private readonly EnemySpawner _enemySpawner;

        public GameState(StateMachine stateMachine, HeroComponents heroComponents, EnemySpawner enemySpawner,
            AbilityFactory abilityFactory) : base(stateMachine)
        {
            _heroComponents = heroComponents.ThrowIfNull();
            _enemySpawner = enemySpawner.ThrowIfNull();
            _abilityFactory = abilityFactory.ThrowIfNull();
        }

        public override void Enter()
        {
            _heroComponents.AbilityContainer.Add(_abilityFactory.Create(AbilityType.SwordStrike));
        }

        public override void Update()
        {
            _enemySpawner.Update();
            //_спавнерУсилений.Update();
        }

        public override void Exit()
        {
            throw new NotImplementedException();
        }
    }
}
