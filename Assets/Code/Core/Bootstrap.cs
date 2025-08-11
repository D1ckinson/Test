using Assets.Code;
using Assets.Code.AbilitySystem;
using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Code.Shop;
using Assets.Code.Spawners;
using Assets.Code.Tools;
using Assets.Scripts.Configs;
using Assets.Scripts.Factories;
using Assets.Scripts.State_Machine;
using Assets.Scripts.Tools;
using Assets.Scripts.Ui;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Assets.Scripts
{
    [RequireComponent(typeof(ExperienceDistiller))]
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private LevelSettings _levelSettings;
        [SerializeField] private UIConfig _uIConfig;

        private StateMachine _stateMachine;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_levelSettings.IsNull())
            {
                return;
            }

            Gizmos.color = Color.red;
            GameAreaSettings gameAreaSettings = _levelSettings.GameAreaSettings;

            Gizmos.DrawSphere(gameAreaSettings.Center, 1);
            CustomGizmos.DrawCircle(gameAreaSettings.Center, gameAreaSettings.Radius, Color.red);
        }
#endif

        private void Awake()
        {
            PlayerData playerData = YG2.saves.Load();
            HeroLevel heroLevel = new(_levelSettings.CalculateExperienceForNextLevel);
            GameAreaSettings gameAreaSettings = _levelSettings.GameAreaSettings;
            HeroComponents heroComponents = new HeroFactory(_levelSettings.HeroConfig).Create(gameAreaSettings.Center);
            SessionData sessionData = new(heroLevel, heroComponents);
            playerData.Wallet.Add(10000);
            GetComponent<ExperienceDistiller>().Initialize(heroLevel);////////////////////////////////

            Dictionary<AbilityType, AbilityConfig> abilities = _levelSettings.AbilityConfigs;

            AbilityFactory abilityFactory = new(abilities, heroComponents.transform);
            LootFactory lootFactory = new(_levelSettings.Loots, playerData.Wallet, sessionData.HeroLevel);
            EnemyFactory enemyFactory = new(_levelSettings.EnemyConfigs, lootFactory, heroComponents.transform, _levelSettings.EnemySpawnerSettings, gameAreaSettings);

            LevelUpWindow levelUpWindow = new(_uIConfig.LevelUpCanvas, _uIConfig.LevelUpButton);
            new UpgradeTrigger(heroLevel, abilities, heroComponents.AbilityContainer, levelUpWindow, abilityFactory, playerData.AbilityUnlockLevel);

            EnemySpawner enemySpawner = new(enemyFactory, _levelSettings.SpawnTypesByTime);

            MenuWindow menu = new(_uIConfig.MenuButton, _uIConfig.MenuCanvas);
            ShopWindow shop = new(playerData, _levelSettings, _levelSettings.UpgradeCost, _uIConfig.ShopCanvas, _uIConfig.ShopButton);

            _stateMachine = new();
            _stateMachine
                .AddState(new MenuState(_stateMachine, menu, shop))
                .AddState(new GameState(_stateMachine, heroComponents, enemySpawner, abilityFactory));

            _stateMachine.SetState<MenuState>();
        }

        private void Update()
        {
            _stateMachine.Update();
        }
    }
}
