using Assets.Code.Tools;
using Assets.Code.Ui;
using Assets.Code.Ui.Windows;
using YG;

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
            _uiFactory.Create<ShopWindow>().ExitButton.Subscribe(ShowMenu);
            _uiFactory.Hide<ShopWindow>();
            _uiFactory.Create<LeaderboardWindow>().ExitButton.Subscribe(ShowMenu);
            _uiFactory.Hide<LeaderboardWindow>();

            MenuWindow menuWindow = _uiFactory.Create<MenuWindow>();
            menuWindow.ShopButton.Subscribe(ShowShop);
            menuWindow.PlayButton.Subscribe(SetState<GameState>);
            menuWindow.SetActive(true);
            menuWindow.LeaderboardButton.Subscribe(ShowLeaderboard);

            _uiFactory.Create<FadeWindow>().Hide();
        }

        private void ShowMenu()
        {
            _uiFactory.Create<MenuWindow>();
        }

        private void ShowShop()
        {
            _uiFactory.Hide<MenuWindow>();
            _uiFactory.Create<ShopWindow>();
        }

        private void ShowLeaderboard()
        {
            _uiFactory.Hide<MenuWindow>();
            LeaderboardWindow leaderboard = _uiFactory.Create<LeaderboardWindow>();
            leaderboard.Leaderboard.UpdateLB();
        }

        public override void Exit()
        {
            _uiFactory.Create<ShopWindow>().ExitButton.Unsubscribe(ShowMenu);
            _uiFactory.Create<MenuWindow>().ShopButton.UnsubscribeAll();
            _uiFactory.HideAll();
        }

        public override void Update() { }
    }
}
