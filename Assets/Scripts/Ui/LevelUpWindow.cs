using Assets.Scripts.ExperienceSystem;
using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ui
{
    public class LevelUpWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _ui;//
        [SerializeField] private List<Button> _buttons;

        private HeroLevel _heroLevel;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _buttons.ThrowIfNull();
            _buttons.ForEach(button => button.ThrowIfNull());
        }
#endif

        private void Awake()
        {
            _ui.SetActive(false);
            _buttons.ForEach(button => button.onClick.AddListener(Hide));
        }

        private void OnDestroy()
        {
            if (_heroLevel != null)
            {
                _heroLevel.LevelUp -= Show;
            }

            if (_buttons == null)
            {
                return;
            }

            foreach (Button button in _buttons)
            {
                if (button != null)
                {
                    button.onClick.RemoveListener(Hide);
                }
            }
        }

        public void Initialize(HeroLevel heroLevel)
        {
            heroLevel.ThrowIfNull();
            _heroLevel = heroLevel;
            _heroLevel.LevelUp += Show;
        }

        private void Show(int level)
        {
            level.ThrowIfZeroOrLess();
            Debug.Log(Time.timeScale);
            Time.timeScale = Constants.Zero;

            _ui.SetActive(true);
        }

        private void Hide()
        {
            _ui.SetActive(false);
            Time.timeScale = Constants.One;
        }
    }
}
