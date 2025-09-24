using Assets.Code.Tools;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Ui.Windows
{
    public class FadeWindow : BaseWindow
    {
        [SerializeField] private Image _image;
        [SerializeField][Min(0)] private float _fadeDuration = 0.5f;

        private Sequence _showSequence;
        private Sequence _hideSequence;

        private Action _onShow;
        private Action _onHide;

        private bool IsVisible => gameObject.activeSelf && _image.color.a == Constants.One;

        private void Awake()
        {
            _showSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Append(_image.DOFade(Constants.One, _fadeDuration))
                .OnComplete(() => _onShow?.Invoke());

            _hideSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Append(_image.DOFade(Constants.Zero, _fadeDuration))
                .OnComplete(() =>
                {
                    _onHide?.Invoke();
                    gameObject.SetActive(false);
                });
        }

        private void OnDestroy()
        {
            _showSequence?.Kill();
            _hideSequence?.Kill();
        }

        public void Show(Action onComplete = null)
        {
            if (IsVisible || _showSequence.IsPlaying())
            {
                onComplete?.Invoke();

                return;
            }

            _onShow = onComplete;
            gameObject.SetActive(true);

            DOTween.Kill(this);
            _showSequence.Restart();
        }

        public void Hide(Action onComplete = null)
        {
            if (IsVisible == false || _hideSequence.IsPlaying())
            {
                onComplete?.Invoke();

                return;
            }

            _onHide = onComplete;

            DOTween.Kill(this);
            _hideSequence.Restart();
        }
    }
}