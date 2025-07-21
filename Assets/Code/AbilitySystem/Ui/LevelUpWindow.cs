using Assets.Code;
using Assets.Code.AbilitySystem.Ui;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Transform layoutGroup = _canvas.GetComponentInChildren<LayoutGroup>().ThrowIfNull().transform;

            _buttons = new LevelUpButton[]
            {
                Object.Instantiate(buttonPrefab,layoutGroup , false),
                Object.Instantiate(buttonPrefab,layoutGroup ,false),
                Object.Instantiate(buttonPrefab,layoutGroup,false)
            };

            foreach (LevelUpButton button in _buttons)
            {
                button.gameObject.SetActive(false);
            }

            _canvas.gameObject.SetActive(false);
        }

        public event Action<AbilityType> UpgradeChosen;

        public void Show(Dictionary<AbilityConfig, int> abilities, int level)
        {
            abilities.ThrowIfCollectionNullOrEmpty();

            Time.timeScale = Constants.Zero;
            AbilityConfig[] configs = abilities.Keys.ToArray();

            for (int i = Constants.Zero; i < abilities.Count; i++)
            {
                LevelUpButton button = _buttons[i];
                AbilityConfig config = configs[i];
                string description = PrepareDescription(config, abilities[config]);

                button.SetDescription(description, config.Image);
                button.Subscribe(() => Callback(config.Type));
                button.gameObject.SetActive(true);
            }

            _canvas.gameObject.SetActive(true);
        }

        private void Callback(AbilityType abilityType)
        {
            foreach (LevelUpButton button in _buttons)
            {
                button.UnsubscribeAll();
                button.gameObject.SetActive(false);
            }

            _canvas.gameObject.SetActive(false);
            UpgradeChosen?.Invoke(abilityType);

            Time.timeScale = Constants.One;
        }

        private string PrepareDescription(AbilityConfig config, int level)
        {
            return "Заглушка";
        }
    }
}
