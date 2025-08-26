using Assets.Code.Data;
using Assets.Code.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Code.Shop
{
    public class ShopOption : MonoBehaviour
    {
        [SerializeField] private Image _abilityIcon;
        [SerializeField] private Image _goldIcon;
        [SerializeField] private TMP_Text _abilityName;
        [SerializeField] private TMP_Text _levelDescription;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _buyText;
        [SerializeField] private TMP_Text _costText;

        public int Cost => _costText.text.ParseOrThrow();
        public bool IsMaxed => _buyText.text == UIText.LevelMax;

        public void Initialize(Sprite abilityIcon, string abilityName)
        {
            _abilityIcon.sprite = abilityIcon.ThrowIfNull();
            _abilityName.text = abilityName.ThrowIfNullOrEmpty();
        }

        public void Subscribe(UnityAction onClick)
        {
            _button.Subscribe(onClick);
        }

        public void SetDescription(int level, string buyText, int cost)
        {
            _levelDescription.SetText(UIText.Level + level.ThrowIfNegative().ToString());
            _buyText.SetText(buyText.ThrowIfNullOrEmpty());
            _costText.SetText(cost.ThrowIfNegative().ToString());
        }

        public void SetColor(Color color)
        {
            ColorBlock colorBlock = _button.colors;
            colorBlock.normalColor = color;
            colorBlock.disabledColor = color;
            _button.colors = colorBlock;

            if (color == Color.red)
            {
                _button.interactable = false;
            }
            else if (color == Color.yellow)
            {
                _button.interactable = false;
                _goldIcon.SetActive(false);
                _costText.SetActive(false);
            }
            else
            {
                _button.interactable = true;
            }
        }
    }
}
