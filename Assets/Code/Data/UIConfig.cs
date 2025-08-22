using Assets.Code.Shop;
using Assets.Code.Ui;
using Assets.Code.Ui.Buttons;
using Assets.Code.Ui.LevelUp;
using Assets.Code.Ui.Windows;
using Assets.Scripts.State_Machine;
using UnityEngine;

namespace Assets.Code.Data
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
        public BaseWindow FadeWindow;
        public FPSWindow FPSWindow;
        public MenuWindow1 MenuWindow;
    }
}
