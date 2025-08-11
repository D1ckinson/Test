using Assets.Code.Shop;
using Assets.Scripts.Tools;

namespace Assets.Scripts.State_Machine
{
    public class MenuState : State
    {
        private readonly MenuWindow _menu;
        private readonly ShopWindow _shop;

        public MenuState(StateMachine stateMachine, MenuWindow menu, ShopWindow shop) : base(stateMachine)
        {
            _menu = menu.ThrowIfNull();
            _shop = shop.ThrowIfNull();

            _menu.Subscribe(ButtonType.Play, () => SetState<GameState>());
            _menu.Subscribe(ButtonType.Shop, () => ShowShop());
        }

        public override void Enter()
        {
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
