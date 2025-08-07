using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Code.AbilitySystem.Ui
{
    public class TextButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void Subscribe(UnityAction call)
        {
            _button.onClick.AddListener(call);
        }

        public void UnsubscribeAll()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
