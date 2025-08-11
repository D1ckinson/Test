using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.State_Machine
{
    public class MenuWindow
    {
        private readonly Dictionary<ButtonType, Button> _buttons;
        private readonly Canvas _canvas;

        public MenuWindow(Button buttonPrefab, Canvas canvasPrefab)
        {
            buttonPrefab.ThrowIfNull();
            _canvas = Object.Instantiate(canvasPrefab.ThrowIfNull());
            Transform layoutGroup = _canvas.GetComponentInChildrenOrThrow<LayoutGroup>().transform;

            _buttons = new()
            {
                [ButtonType.Play] = Object.Instantiate(buttonPrefab, layoutGroup, false),
                [ButtonType.Shop] = Object.Instantiate(buttonPrefab, layoutGroup, false),
                [ButtonType.Leaderboard] = Object.Instantiate(buttonPrefab, layoutGroup, false),
            };

            _buttons[ButtonType.Shop].GetComponentInChildren<TMP_Text>().text = "Магазин";///////////////

            Toggle(false);
        }

        public void Toggle(bool? isActive = null)
        {
            isActive ??= _canvas.IsActive() == false;

            _canvas.SetActive((bool)isActive);
            _buttons.ForEachValues(button => button.SetActive((bool)isActive));
        }

        public void Subscribe(ButtonType buttonType, UnityAction call)
        {
            _buttons.GetValueOrThrow(buttonType.ThrowIfNull()).Subscribe(call.ThrowIfNull());
        }

        public void UnsubscribeAll()
        {
            _buttons.ForEachValues(button => button.UnsubscribeAll());
        }
    }
}
