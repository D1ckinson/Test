using Assets.Code.Data;
using Assets.Code.Tools;
using Assets.Scripts;
using Assets.Scripts.Configs;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Code.Shop
{
    public class ShopWindow
    {
        private readonly PlayerData _playerData;
        private readonly LevelSettings _levelSettings;
        private readonly Canvas _canvas;
        private readonly List<ShopOption> _options;
        private readonly UpgradeCost _upgradeCost;
        private readonly Button _exit;
        private readonly TMP_Text _walletView;

        public ShopWindow(PlayerData playerData, LevelSettings levelSettings, UpgradeCost upgradeCost, Canvas canvas, ShopOption buttonPrefab)
        {
            _playerData = playerData.ThrowIfNull();
            _levelSettings = levelSettings.ThrowIfNull();
            _upgradeCost = upgradeCost.ThrowIfNull().Initialize();

            _canvas = Object.Instantiate(canvas);
            _exit = _canvas.GetComponentInChildrenOrThrow<ExitButton>().Button;
            _walletView = _canvas.GetComponentInChildrenOrThrow<WalletView>().Text;
            _exit.Subscribe(OnExit);

            Transform layoutGroup = _canvas.GetComponentInChildrenOrThrow<LayoutGroup>().transform;
            _options = CreateOptions(buttonPrefab.ThrowIfNull(), layoutGroup);

            UpdateElementsDescription(_playerData.Wallet.CoinsQuantity);

            _canvas.SetActive(false);
            _options.ForEach(option => option.SetActive(false));
        }

        ~ShopWindow()
        {
            _playerData.Wallet.ValueChanged -= UpdateElementsDescription;
            _exit.UnsubscribeAll();
        }

        public event Action<ShopWindow> Exiting;

        public void Toggle(bool? isActive = null)
        {
            isActive ??= _canvas.IsActive() == false;

            _canvas.SetActive((bool)isActive);
            _options.ForEach(option => option.SetActive((bool)isActive));
            _walletView.SetText(_playerData.Wallet.CoinsQuantity.ToString());

            switch (isActive)
            {
                case true:
                    _playerData.Wallet.ValueChanged += UpdateElementsDescription;
                    break;
                case false:
                    _playerData.Wallet.ValueChanged -= UpdateElementsDescription;
                    break;
                default:
                    break;
            }
        }

        private void UpdateElementsDescription(float coinsQuantity)
        {
            _walletView.text = coinsQuantity.ToString();
            _options.ForEach(SetOptionColor);
        }

        private void OnExit()
        {
            Toggle(false);
            Exiting?.Invoke(this);
        }

        private void SetOptionColor(ShopOption option)
        {
            if (option.IsMaxed)
            {
                option.SetColor(Color.yellow);
            }
            else if (option.Cost > _playerData.Wallet.CoinsQuantity)
            {
                option.SetColor(Color.red);
            }
            else
            {
                option.SetColor(Color.green);
            }
        }

        private List<ShopOption> CreateOptions(ShopOption prefab, Transform parent)
        {
            List<ShopOption> options = new();
            Dictionary<AbilityType, AbilityConfig> abilityConfigs = _levelSettings.AbilityConfigs;
            IEnumerable<AbilityType> abilityTypes = Constants.GetEnums<AbilityType>();

            foreach (AbilityType abilityType in abilityTypes)
            {
                ShopOption option = Object.Instantiate(prefab, parent, false);

                AbilityConfig abilityConfig = abilityConfigs.GetValueOrThrow(abilityType);

                int availableLevel = _playerData.AbilityUnlockLevel.GetValueOrThrow(abilityType);
                int cost = _upgradeCost.GetCost(abilityType, availableLevel + Constants.One);

                string buyText = availableLevel == abilityConfig.MaxLevel ? UIText.LevelMax : UIText.Upgrade;

                option.Initialize(abilityConfig.Icon, abilityConfig.Name);
                SetDescription(abilityType, option);

                SetOptionColor(option);
                option.Subscribe(() => IncreaseUnlockLevel(abilityType, option));

                options.Add(option);
            }

            return options;
        }

        private void IncreaseUnlockLevel(AbilityType abilityType, ShopOption option)
        {
            int unlockLevel = _playerData.AbilityUnlockLevel[abilityType] + Constants.One;

            _playerData.Wallet.Spend(_upgradeCost.GetCost(abilityType, unlockLevel));
            _playerData.AbilityUnlockLevel[abilityType] = unlockLevel;
            SetDescription(abilityType, option);
        }

        private void SetDescription(AbilityType abilityType, ShopOption option)
        {
            AbilityConfig abilityConfig = _levelSettings.AbilityConfigs.GetValueOrThrow(abilityType);

            int availableLevel = _playerData.AbilityUnlockLevel.GetValueOrThrow(abilityType);
            int cost = _upgradeCost.GetCost(abilityType, availableLevel + Constants.One);

            string buyText = availableLevel >= abilityConfig.MaxLevel ? UIText.LevelMax : UIText.Upgrade;
            option.SetDescription(availableLevel, buyText, cost);
        }
    }
}
