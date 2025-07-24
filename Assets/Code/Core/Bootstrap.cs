using Assets.Code;
using Assets.Code.AbilitySystem;
using Assets.Code.AmplificationSystem;
using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Code.Data;
using Assets.Scripts.Configs;
using Assets.Scripts.Factories;
using Assets.Scripts.State_Machine;
using Assets.Scripts.Tools;
using Assets.Scripts.Ui;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private LevelSettings _levelSettings;
        [SerializeField] private UIConfig _uIConfig;

        private PlayerData _playerData;
        private StateMachine _stateMachine;

        private void OnDrawGizmos()
        {
            if (_levelSettings == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            GameAreaSettings gameAreaSettings = _levelSettings.GameAreaSettings;

            Gizmos.DrawSphere(gameAreaSettings.Center, 1);
            CustomGizmos.DrawCircle(gameAreaSettings.Center, gameAreaSettings.Radius, Color.red);
        }

        private void Awake()
        {
            _playerData = new(new(), new(_levelSettings));
            _stateMachine = new();

            GameAreaSettings gameAreaSettings = _levelSettings.GameAreaSettings;
            HeroComponents heroComponents = new HeroFactory(_levelSettings.HeroConfig).Create(gameAreaSettings.Center);
            _playerData.SetHeroTransform(heroComponents);

            AbilityFactory abilityFactory = new(_levelSettings.AbilityConfigs, heroComponents.transform);
            LevelUpWindow levelUpWindow = new(_uIConfig.LevelUpCanvas, _uIConfig.LevelUpButton);
            LootFactory lootFactory = new(_levelSettings.Loots, _playerData);
            BuffFactory buffFactory = new(_levelSettings.BuffConfigs, heroComponents, _playerData);
            EnemyFactory enemyFactory = new(_levelSettings.EnemyConfigs, lootFactory, heroComponents.transform, _levelSettings.EnemySpawnerSettings, gameAreaSettings);
            new UpgradeTrigger(_playerData.HeroExperience, _playerData.HeroComponents.AbilityContainer, levelUpWindow, abilityFactory, buffFactory, heroComponents.BuffContainer);

            GameTimer gameTimer = new();

            _stateMachine
                .AddState(new MenuState(_stateMachine))
                .AddState(new GameState(_stateMachine, heroComponents, enemyFactory, abilityFactory, gameTimer, _levelSettings.EnemyTypeByTime));

            _stateMachine.SetState<MenuState>();
        }

        private void Update()
        {
            _stateMachine.Update();
        }
    }
}
