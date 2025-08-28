using Assets.Code;
using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Code.InputActions;
using Assets.Code.Spawners;
using Assets.Code.Tools;
using Assets.Code.Ui;
using Assets.Code.Ui.Windows;
using YG;

namespace Assets.Scripts.State_Machine
{
    public class GameState : State
    {
        private readonly HeroComponents _hero;
        private readonly AbilityFactory _abilityFactory;
        private readonly EnemySpawner _enemySpawner;
        private readonly UiFactory _uiFactory;
        private readonly string _resurrectRewardId = "resurrect";
        private readonly Timer _timer;
        private readonly IInputService _inputService;

        public GameState(StateMachine stateMachine, HeroComponents heroComponents, EnemySpawner enemySpawner,
            AbilityFactory abilityFactory, UiFactory uiFactory, IInputService inputService) : base(stateMachine)
        {
            _hero = heroComponents.ThrowIfNull();
            _enemySpawner = enemySpawner.ThrowIfNull();
            _abilityFactory = abilityFactory.ThrowIfNull();
            _uiFactory = uiFactory.ThrowIfNull();
            _inputService = inputService.ThrowIfNull();
            _timer = new();
        }

        public override void Enter()
        {
            _hero.AbilityContainer.Add(_abilityFactory.Create(AbilityType.SwordStrike));
            _hero.AbilityContainer.Run();
            _hero.Health.Died += ShowDeathWindow;
            _hero.LootCollector.Run();
            _hero.CharacterMovement.Run();
            _enemySpawner.Run();
            _timer.Start();
        }

        public override void Update()
        {
            if (true)
            {

            }
        }

        public override void Exit()
        {
            DeathWindow deathWindow = _uiFactory.Create<DeathWindow>();
            deathWindow.BackToMenuButton.UnsubscribeAll();
            deathWindow.ContinueForAddButton.UnsubscribeAll();
            deathWindow.SetActive(false);
            deathWindow.ContinueForAddButton.interactable = true;

            _hero.Health.Died -= ShowDeathWindow;
            _hero.AbilityContainer.RemoveAll();
            _hero.LootCollector.Stop();
            _hero.LootCollector.TransferGold();
            _hero.HeroLevel.Reset();
            _hero.Health.ResetValue();
            _hero.SetActive(true);
            _hero.SetDefaultPosition();

            _timer.Stop();
            _enemySpawner.Reset();
        }

        private void ShowDeathWindow()
        {
            _hero.LootCollector.Stop();
            _hero.CharacterMovement.Stop();
            _hero.AbilityContainer.Stop();

            _enemySpawner.Pause();
            _timer.Pause();

            DeathWindow deathWindow = _uiFactory.Create<DeathWindow>();
            deathWindow.CoinsQuantity.SetText(_hero.LootCollector.CollectedGold.ToString(StringFormat.WholeNumber));
            deathWindow.MinutesQuantity.SetText(_timer.Duration.ToMinutesString());
            deathWindow.BackToMenuButton.Subscribe(() => _uiFactory.Create<FadeWindow>().Show(SetState<MenuState>));
            deathWindow.ContinueForAddButton.Subscribe(ShowAdd);
        }

        private void ShowAdd()
        {
            YG2.RewardedAdvShow(_resurrectRewardId, Resurrect);
        }

        private void Resurrect()
        {
            _hero.LootCollector.Run();
            _hero.CharacterMovement.Run();
            _hero.AbilityContainer.Run();
            _hero.Health.ResetValue();

            _enemySpawner.Continue();
            _timer.Continue();

            DeathWindow deathWindow = _uiFactory.Create<DeathWindow>();
            deathWindow.ContinueForAddButton.interactable = false;
            deathWindow.SetActive(false);
            deathWindow.BackToMenuButton.UnsubscribeAll();
            deathWindow.ContinueForAddButton.UnsubscribeAll();
        }
    }
}
