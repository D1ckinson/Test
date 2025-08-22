using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Tools
{
    public class ToggleButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _turnOnSprite;
        [SerializeField] private Sprite _turnOffSprite;

        public bool IsOn { get; private set; }

        public event Action<bool> OnClick;

        public void Toggle(bool isOn)
        {
            _icon.sprite = isOn ? _turnOnSprite : _turnOffSprite;
            OnClick?.Invoke(isOn);
        }
    }
}
