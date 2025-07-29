using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
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

        public void SetDescription(List<string> text, Sprite image)
        {
            _text.text = text.ThrowIfCollectionNullOrEmpty().ToTextWithNewLines();
            _image.sprite = image.ThrowIfNull();
        }

        public void Subscribe(Action call)
        {
            _button.onClick.AddListener(() => call?.Invoke());
        }

        public void UnsubscribeAll()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
