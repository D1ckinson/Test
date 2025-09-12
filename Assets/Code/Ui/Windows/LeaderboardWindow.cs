using Assets.Code.Tools;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Assets.Code.Ui.Windows
{
    public class LeaderboardWindow : BaseWindow
    {
        [field: SerializeField] public Button ExitButton { get; private set; }
        [field: SerializeField] public LeaderboardYG Leaderboard { get; private set; }

        private void OnDestroy()
        {
            ExitButton.UnsubscribeAll();
        }

        public LeaderboardWindow Initialize()
        {
            ExitButton.Subscribe(() => this.SetActive(false));

            return this;
        }
    }
}