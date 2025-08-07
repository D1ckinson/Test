using Assets.Code.Ui.LevelUp;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ui
{
    [CreateAssetMenu(menuName = "Game/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        [Header("Menu")]
        public Canvas MenuCanvas;
        public Button MenuButton;

        [Header("LevelUp Window")]
        public Canvas LevelUpCanvas;
        public LevelUpButton LevelUpButton;
    }
}
