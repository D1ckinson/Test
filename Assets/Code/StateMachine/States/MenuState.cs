using Assets.Code.Shop;
using Assets.Code.Tools;
using Assets.Code.Ui;
using Assets.Code.Ui.Windows;
using System;

namespace Assets.Scripts.State_Machine
{
    public class MenuState : State
    {
        private readonly UiFactory _uiFactory;

        public MenuState(StateMachine stateMachine, UiFactory uiFactory) : base(stateMachine)
        {
            _uiFactory = uiFactory.ThrowIfNull();
        }

        public override void Enter()
        {
            _uiFactory.Create<ShopWindow1>().ExitButton.Subscribe(ShowMenu);
            _uiFactory.Hide<ShopWindow1>();

            MenuWindow1 menuWindow = _uiFactory.Create<MenuWindow1>();
            menuWindow.ShopButton.Subscribe(ShowShop);
            menuWindow.PlayButton.Subscribe(SetState<GameState>);
            menuWindow.SetActive(true);

            _uiFactory.Create<FadeWindow>().Hide();
        }

        private void ShowMenu()
        {
            _uiFactory.Create<MenuWindow1>();
        }

        private void ShowShop()
        {
            _uiFactory.Hide<MenuWindow1>();
            _uiFactory.Create<ShopWindow1>();
        }

        public override void Exit()
        {
            _uiFactory.Create<ShopWindow1>().ExitButton.Unsubscribe(ShowMenu);
            _uiFactory.Create<MenuWindow1>().ShopButton.UnsubscribeAll();
            _uiFactory.HideAll();
        }

        public override void Update()
        {
        }
    }
}
