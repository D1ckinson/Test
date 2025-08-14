using Assets.Code.Shop;
using Assets.Code.Ui;
using Assets.Code.Ui.LevelUp;
using Assets.Scripts.State_Machine;
using UnityEngine;

namespace Assets.Scripts.Ui
{
    [CreateAssetMenu(menuName = "Game/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        [Header("Menu")]
        public Canvas MenuCanvas;
        public TextButton MenuButton;

        [Header("LevelUp Window")]
        public Canvas LevelUpCanvas;
        public LevelUpButton LevelUpButton;

        [Header("Shop")]
        public Canvas ShopCanvas;
        public ShopOption ShopButton;

        [Header("Other")]
        public DeathWindow DeathWindow;
        public TestCanvasUiFactory TestCanvasUiFactory;
    }
}
