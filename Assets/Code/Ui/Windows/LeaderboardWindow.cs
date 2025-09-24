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

        private void Awake()
        {
            ExitButton.Subscribe(Disable);
        }

        private void OnDestroy()
        {
            ExitButton.Unsubscribe(Disable);
        }
    }
}