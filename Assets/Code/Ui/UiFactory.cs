using Assets.Code.Data;
using Assets.Code.Shop;
using Assets.Code.Tools;
using Assets.Code.Ui.Windows;
using Assets.Scripts;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Assets.Code.Ui
{
    public class UiFactory
    {
        private readonly Dictionary<Type, BaseWindow> _windows = new();
        private readonly Dictionary<Type, Func<BaseWindow>> _createMethods;
        private readonly UiCanvas _canvas;
        private readonly UIConfig _uIConfig;
        private readonly Wallet _wallet;
        private readonly Dictionary<AbilityType, AbilityConfig> _abilityConfigs;
        private readonly UpgradeCost _upgradeCost;
        private readonly Dictionary<AbilityType, int> _abilityUnlockLevel;
        private readonly Dictionary<AbilityType, int> _abilityMaxLevel;

        public UiFactory(UIConfig uIConfig, Wallet wallet, UpgradeCost upgradeCost, Dictionary<AbilityType, AbilityConfig> abilityConfigs, Dictionary<AbilityType, int> abilityUnlockLevel, Dictionary<AbilityType, int> abilityMaxLevel)
        {
            _uIConfig = uIConfig.ThrowIfNull();
            _wallet = wallet.ThrowIfNull();
            _upgradeCost = upgradeCost.ThrowIfNull();
            _abilityConfigs = abilityConfigs.ThrowIfNullOrEmpty();
            _abilityUnlockLevel = abilityUnlockLevel.ThrowIfNullOrEmpty();
            _abilityMaxLevel = abilityMaxLevel.ThrowIfNullOrEmpty();
            _canvas = Object.Instantiate(_uIConfig.TestCanvasUiFactory);

            _createMethods = new()
            {
                [typeof(DeathWindow)] = CreateDeathWindow,
                [typeof(FadeWindow)] = CreateFadeWindow,
                [typeof(FPSWindow)] = CreateFPSView,
                [typeof(MenuWindow)] = CreateMenuWindow,
                [typeof(ShopWindow)] = CreateShopWindow,
                [typeof(Joystick)] = CreateJoystick,
                [typeof(LeaderboardWindow)] = CreateLeaderboardWindow,
                [typeof(PauseWindow)] = CreatePauseWindow
            };
        }

        public T Create<T>(bool isActive = true) where T : BaseWindow
        {
            Type windowType = typeof(T);

            if (_windows.TryGetValue(windowType, out BaseWindow window) == false)
            {
                window = _createMethods[windowType].Invoke();
                _windows.Add(windowType, window);
            }

            window.SetActive(isActive);

            return (T)window;
        }

        private BaseWindow CreateFadeWindow()
        {
            return _uIConfig.FadeWindow.Instantiate(_canvas.FadeContainer, false);
        }

        private BaseWindow CreateDeathWindow()
        {
            return _uIConfig.DeathWindow.Instantiate(_canvas.DeathWindowPoint, false).Initialize();
        }

        private BaseWindow CreateFPSView()
        {
            return _uIConfig.FPSWindow.Instantiate(_canvas.Container, false);
        }

        private BaseWindow CreateMenuWindow()
        {
            return _uIConfig.MenuWindow.Instantiate(_canvas.Container, false).Initialize(_wallet);
        }

        private BaseWindow CreateShopWindow()
        {
            ShopWindow shopWindow = _uIConfig.ShopWindow.Instantiate(_canvas.Container, false).Initialize(_abilityUnlockLevel, _abilityMaxLevel, _upgradeCost, _wallet);

            foreach (AbilityType abilityType in Constants.GetEnums<AbilityType>())
            {
                ShopOption shopOption = _uIConfig.ShopOption.Instantiate().Initialize(abilityType);
                shopOption.AbilityIcon.sprite = _abilityConfigs[abilityType].Icon;

                shopWindow.AddOption(shopOption);
            }

            return shopWindow;
        }

        private BaseWindow CreateJoystick()
        {
            return _uIConfig.Joystick.Instantiate(_canvas.Container, false);
        }

        private BaseWindow CreateLeaderboardWindow()
        {
            return _uIConfig.Leaderboard.Instantiate(_canvas.Container, false);
        }

        private BaseWindow CreatePauseWindow()
        {
            return _uIConfig.PauseWindow.Instantiate(_canvas.Container, false);
        }
    }
}