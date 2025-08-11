using Assets.Code;
using Assets.Code.AbilitySystem;
using Assets.Code.Tools;
using Assets.Code.Ui.LevelUp;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Ui
{
    public class LevelUpWindow
    {
        private readonly LevelUpButton[] _buttons;
        private readonly Canvas _canvas;

        public LevelUpWindow(Canvas canvas, LevelUpButton buttonPrefab)
        {
            _canvas = Object.Instantiate(canvas.ThrowIfNull());
            Transform layoutGroup = _canvas.GetComponentInChildrenOrThrow<LayoutGroup>().transform;

            _buttons = new LevelUpButton[]
            {
                Object.Instantiate(buttonPrefab, layoutGroup, false),
                Object.Instantiate(buttonPrefab, layoutGroup, false),
                Object.Instantiate(buttonPrefab, layoutGroup, false)
            };

            _buttons.ForEach(button => button.SetActive(false));
            _canvas.SetActive(false);
        }

        public event Action<AbilityType> UpgradeChosen;

        public void Show(List<UpgradeOption> upgradeOptions, int level = 1)
        {
            upgradeOptions.ThrowIfNullOrEmpty();

            Time.timeScale = Constants.Zero;

            for (int i = Constants.Zero; i < upgradeOptions.Count; i++)
            {
                LevelUpButton button = _buttons[i];
                UpgradeOption upgradeOption = upgradeOptions[i];
                button.SetDescription(upgradeOption.Name, upgradeOption.Icon, upgradeOption.Stats);
                button.Subscribe(() => Callback(upgradeOption.Type));
                button.SetActive(true);
            }

            _canvas.SetActive(true);
        }

        private void Callback(AbilityType abilityType)
        {
            foreach (LevelUpButton button in _buttons)
            {
                button.UnsubscribeAll();
                button.SetActive(false);
            }

            _canvas.SetActive(false);
            UpgradeChosen?.Invoke(abilityType);

            Time.timeScale = Constants.One;
        }
    }
}
