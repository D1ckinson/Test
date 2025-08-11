using Assets.Code.Tools;
using Assets.Scripts.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Code.Shop
{
    public class ShopOption : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _abilityName;
        [SerializeField] private TMP_Text _levelDescription;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _buyText;
        [SerializeField] private TMP_Text _costText;

        public int Cost => _costText.text.ParseOrThrow();
        public bool Maxed => _buyText.text == "Макс.";

        public ShopOption Initialize(Sprite sprite, string abilityName)
        {
            _icon.sprite = sprite.ThrowIfNull();
            _abilityName.text = abilityName.ThrowIfNullOrEmpty();

            return this;
        }

        public void Subscribe(UnityAction onClick)
        {
            _button.Subscribe(onClick);
        }

        public void SetDescription(int level, string buyText, int cost)
        {
            _levelDescription.SetText("Ур." + level.ThrowIfNegative().ToString());
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
            else
            {
                _button.interactable = true;
            }
        }
    }
}
