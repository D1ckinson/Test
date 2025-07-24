using Assets.Scripts.Tools;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.AbilitySystem.Ui
{
    public class LevelUpButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _image;

        public void SetDescription(string text, Sprite image)
        {
            _text.text = text.ThrowIfNull();
            _image.sprite = image.ThrowIfNull();
        }

        public void Subscribe(Action upgradeAction)// поменять название параметра
        {
            _button.onClick.AddListener(() => upgradeAction?.Invoke());
        }

        public void UnsubscribeAll()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
