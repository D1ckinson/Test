﻿using Assets.Code;
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
        private readonly PlayerData _playerData;
        private readonly IInputService _inputService;
        private readonly ITimeService _timeService;

        public GameState(StateMachine stateMachine, HeroComponents heroComponents, EnemySpawner enemySpawner,
            AbilityFactory abilityFactory, UiFactory uiFactory, PlayerData playerData, IInputService inputService
            , ITimeService timeService) : base(stateMachine)
        {
            _hero = heroComponents.ThrowIfNull();
            _enemySpawner = enemySpawner.ThrowIfNull();
            _abilityFactory = abilityFactory.ThrowIfNull();
            _uiFactory = uiFactory.ThrowIfNull();
            _playerData = playerData.ThrowIfNull();
            _inputService = inputService.ThrowIfNull();
            _timeService = timeService.ThrowIfNull();

            _timer = new();
        }

        public override void Enter()
        {
            PauseWindow pauseWindow = _uiFactory.Create<PauseWindow>(false);
            pauseWindow.ExitButton.Subscribe(OnExit);
            pauseWindow.ContinueButton.Subscribe(_timeService.Unpause);

            DeathWindow deathWindow = _uiFactory.Create<DeathWindow>(false);
            deathWindow.BackToMenuButton.Subscribe(OnExit);
            deathWindow.ContinueForAddButton.Subscribe(ShowAdd);

            _hero.AbilityContainer.Add(_abilityFactory.Create(AbilityType.SwordStrike));
            _hero.AbilityContainer.Run();
            _hero.Health.Died += ShowDeathWindow;
            _hero.LootCollector.Run();
            _hero.CharacterMovement.Run();
            _enemySpawner.Run();
            _timer.Start();

            _inputService.BackPressed += Pause;
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            DeathWindow deathWindow = _uiFactory.Create<DeathWindow>(false);
            deathWindow.BackToMenuButton.Unsubscribe(OnExit);
            deathWindow.ContinueForAddButton.Unsubscribe(ShowAdd);
            deathWindow.ContinueForAddButton.interactable = true;

            PauseWindow pauseWindow = _uiFactory.Create<PauseWindow>(false);
            pauseWindow.ExitButton.Subscribe(OnExit);
            pauseWindow.ContinueButton.Subscribe(_timeService.Unpause);

            _hero.Health.Died -= ShowDeathWindow;
            _hero.AbilityContainer.RemoveAll();
            _hero.LootCollector.Stop();
            _hero.LootCollector.TransferGold();
            _hero.HeroLevel.Reset();
            _hero.Health.ResetValue();
            _hero.SetDefaultPosition();

            if (_timer.Duration > _playerData.ScoreRecord)
            {
                _playerData.ScoreRecord = _timer.Duration;
                YG2.SetLBTimeConvert(Constants.LeaderboardName, _playerData.ScoreRecord);
            }

            _inputService.BackPressed -= Pause;
            _timer.Stop();
            _enemySpawner.Reset();
        }

        private void OnExit()
        {
            _timeService.Unpause();
            _uiFactory.Create<FadeWindow>().Show(SetState<MenuState>);
        }

        private void Pause()
        {
            _timeService.Pause();
            _uiFactory.Create<PauseWindow>();
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

            _uiFactory.Create<DeathWindow>(false).ContinueForAddButton.interactable = false;
        }
    }
}
