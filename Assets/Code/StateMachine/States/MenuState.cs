using Assets.Code.Shop;
using Assets.Code.Ui.Windows;
using Assets.Scripts.Tools;

namespace Assets.Scripts.State_Machine
{
    public class MenuState : State
    {
        private readonly MenuWindow _menu;
        private readonly ShopWindow _shop;
        private readonly UiFactory _uiFactory;

        public MenuState(StateMachine stateMachine, MenuWindow menu, ShopWindow shop, UiFactory uiFactory) : base(stateMachine)
        {
            _menu = menu.ThrowIfNull();
            _shop = shop.ThrowIfNull();
            _uiFactory = uiFactory.ThrowIfNull();

            _menu.Subscribe(ButtonType.Play, () => SetState<GameState>());
            _menu.Subscribe(ButtonType.Shop, () => ShowShop());
        }

        public override void Enter()
        {
            _uiFactory.Create<FadeWindow>().Hide();
            _menu.Toggle(true);
        }

        public override void Exit()
        {
            _menu.Toggle(false);
        }

        public override void Update()
        {
        }

        private void ShowShop()
        {
            _menu.Toggle(false);
            _shop.Toggle(true);
            _shop.Exiting += TurnOnMaiMenu;
        }

        private void TurnOnMaiMenu(ShopWindow shopWindow)
        {
            _menu.Toggle(true);
            shopWindow.Exiting -= TurnOnMaiMenu;
        }
    }
}
