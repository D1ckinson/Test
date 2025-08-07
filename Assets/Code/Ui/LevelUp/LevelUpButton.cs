using Assets.Code.AbilitySystem.Ui;
using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Ui.LevelUp
{
    public class LevelUpButton : TextButton
    {
        [SerializeField] private Image _image;

        public void SetDescription(List<string> text, Sprite image)
        {
            SetText(text.ThrowIfNullOrEmpty().ToWrapText());
            _image.sprite = image.ThrowIfNull();
        }
    }
}
