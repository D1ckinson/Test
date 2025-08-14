using Assets.Code;
using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Code.Spawners;
using Assets.Code.Tools;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.State_Machine
{
    public class GameState : State
    {
        private const int SecondsInOneMinute = 60;

        private readonly HeroComponents _heroComponents;
        private readonly AbilityFactory _abilityFactory;
        private readonly EnemySpawner _enemySpawner;
        private readonly UiFactory _uiFactory;
        private readonly string _format = "0.##";

        private float _survivedTime;

        public GameState(StateMachine stateMachine, HeroComponents heroComponents,
            EnemySpawner enemySpawner, AbilityFactory abilityFactory, UiFactory uiFactory) : base(stateMachine)
        {
            _heroComponents = heroComponents.ThrowIfNull();
            _enemySpawner = enemySpawner.ThrowIfNull();
            _abilityFactory = abilityFactory.ThrowIfNull();
            _uiFactory = uiFactory.ThrowIfNull();
        }

        public override void Enter()
        {
            _heroComponents.AbilityContainer.Add(_abilityFactory.Create(AbilityType.SwordStrike));
            _heroComponents.Health.Died += ShowDeathWindow;
            _survivedTime = Constants.Zero;
        }

        public override void Update()
        {
            _enemySpawner.Update();
            _survivedTime += Time.deltaTime;
        }

        public override void Exit()
        {
            _heroComponents.Health.Died -= ShowDeathWindow;
            _heroComponents.AbilityContainer.RemoveAll();
        }

        private void ShowDeathWindow()
        {
            DeathWindow deathWindow = _uiFactory.Create<DeathWindow>();
            //deathWindow.CoinsQuantity =;
            deathWindow.MinutesQuantity.SetText((_survivedTime / SecondsInOneMinute).ToString(_format));
            deathWindow.BackToMenuButton.Subscribe(() => ReturnToMenuState(deathWindow));
        }

        private void ReturnToMenuState(DeathWindow deathWindow)
        {
            deathWindow.BackToMenuButton.UnsubscribeAll();
            _heroComponents.AbilityContainer.RemoveAll();
            _enemySpawner.Reset();
            SetState<MenuState>();
        }
    }
}
