using Assets.Code.Tools;
using Assets.Code.Ui;
using Assets.Code.Ui.Windows;

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
            _uiFactory.Create<FadeWindow>().Hide(OnHide);

            void OnHide()
            {
                _uiFactory.Create<ShopWindow>(false).ExitButton.Subscribe(ShowMenu);
                _uiFactory.Create<LeaderboardWindow>(false).ExitButton.Subscribe(ShowMenu);

                MenuWindow menuWindow = _uiFactory.Create<MenuWindow>();
                menuWindow.ShopButton.Subscribe(ShowShop);
                menuWindow.PlayButton.Subscribe(SetState<GameState>);
                menuWindow.LeaderboardButton.Subscribe(ShowLeaderboard);
            }
        }

        private void ShowMenu()
        {
            _uiFactory.Create<MenuWindow>();
        }

        private void ShowShop()
        {
            _uiFactory.Create<ShopWindow>();
        }

        private void ShowLeaderboard()
        {
            LeaderboardWindow leaderboard = _uiFactory.Create<LeaderboardWindow>();
            leaderboard.Leaderboard.UpdateLB();
        }

        public override void Exit()
        {
            _uiFactory.Create<ShopWindow>(false).ExitButton.Unsubscribe(ShowMenu);
            _uiFactory.Create<LeaderboardWindow>(false).ExitButton.Unsubscribe(ShowMenu);

            MenuWindow menuWindow = _uiFactory.Create<MenuWindow>(false);
            menuWindow.ShopButton.Unsubscribe(ShowShop);
            menuWindow.PlayButton.Unsubscribe(SetState<GameState>);
            menuWindow.LeaderboardButton.Unsubscribe(ShowLeaderboard);
        }

        public override void Update() { }
    }
}
