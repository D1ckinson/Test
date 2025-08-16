using Assets.Code;
using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Code.Spawners;
using Assets.Code.Tools;
using Assets.Code.Ui.Windows;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.State_Machine
{
    public class GameState : State
    {
        private readonly HeroComponents _heroComponents;
        private readonly AbilityFactory _abilityFactory;
        private readonly EnemySpawner _enemySpawner;
        private readonly UiFactory _uiFactory;

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
            _heroComponents.LootCollector.Run();
            _survivedTime = Constants.Zero;
            _enemySpawner.Run();
        }

        public override void Update()
        {
            _survivedTime += Time.deltaTime;
        }

        public override void Exit()
        {
            DeathWindow deathWindow = _uiFactory.Create<DeathWindow>();
            deathWindow.BackToMenuButton.UnsubscribeAll();
            deathWindow.SetActive(false);

            _heroComponents.Health.Died -= ShowDeathWindow;
            _heroComponents.AbilityContainer.RemoveAll();
            _heroComponents.LootCollector.Stop();
            _heroComponents.LootCollector.TransferGold();
            _heroComponents.HeroLevel.Reset();
            _heroComponents.SetActive(true);
            _heroComponents.SetDefaultPosition();

            _enemySpawner.Reset();
        }

        private void ShowDeathWindow()
        {
            _heroComponents.LootCollector.Stop();
            _enemySpawner.Pause();
            DeathWindow deathWindow = _uiFactory.Create<DeathWindow>();

            deathWindow.CoinsQuantity.SetText(_heroComponents.LootCollector.CollectedGold.ToString(StringFormat.WholeNumber));
            deathWindow.MinutesQuantity.SetText(_survivedTime.ToMinutesString());
            deathWindow.BackToMenuButton.Subscribe(() => _uiFactory.Create<FadeWindow>().Show(SetState<MenuState>));
        }
    }
}
