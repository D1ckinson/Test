using Assets.Code.AbilitySystem.Ui;
using UnityEngine;

namespace Assets.Scripts.Ui
{
    [CreateAssetMenu(menuName = "Game/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        [Header("Menu")]

        [Header("LevelUp Window")]
        public Canvas LevelUpCanvas;
        public LevelUpButton LevelUpButton;
    }
}
