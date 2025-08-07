using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.State_Machine
{
    public class MenuState : State
    {
        private readonly MenuWindow _menuWindow;

        public MenuState(StateMachine stateMachine, MenuWindow menuWindow) : base(stateMachine)
        {
            _menuWindow = menuWindow.ThrowIfNull();
            _menuWindow.Subscribe(ButtonType.Play, () => SetState<GameState>());
        }

        public override void Enter()
        {
            _menuWindow.Toggle(true);
        }

        public override void Exit()
        {
            _menuWindow.Toggle(false);
        }

        public override void Update()
        {
        }
    }

    public class MenuWindow
    {
        private readonly Dictionary<ButtonType, Button> _buttons;
        private readonly Canvas _canvas;

        public MenuWindow(Button buttonPrefab, Canvas canvasPrefab)
        {
            buttonPrefab.ThrowIfNull();
            _canvas = Object.Instantiate(canvasPrefab.ThrowIfNull());
            Transform layoutGroup = _canvas.GetComponentInChildren<LayoutGroup>().ThrowIfNull().transform;

            _buttons = new()
            {
                [ButtonType.Play] = Object.Instantiate(buttonPrefab, layoutGroup, false),
                [ButtonType.Shop] = Object.Instantiate(buttonPrefab, layoutGroup, false),
                [ButtonType.Leaderboard] = Object.Instantiate(buttonPrefab, layoutGroup, false),
            };

            Toggle(false);
        }

        public bool IsEnable => _canvas.gameObject.activeSelf;

        public void Toggle(bool? isActive = null)
        {
            isActive ??= IsEnable == false;

            _canvas.SetActive((bool)isActive);
            _buttons.Values.ForEach(button => button.SetActive((bool)isActive));
        }

        public void Subscribe(ButtonType buttonType, UnityAction call)
        {
            _buttons.GetValueOrThrow(buttonType.ThrowIfNull()).onClick.AddListener(call.ThrowIfNull());
        }

        public void UnsubscribeAll()
        {
            _buttons.Values.ForEach(button => button.onClick.RemoveAllListeners());
        }
    }

    public enum ButtonType
    {
        Play,
        Continue,
        Upgrade,
        Shop,
        Leaderboard,
        Exit
    }
}
