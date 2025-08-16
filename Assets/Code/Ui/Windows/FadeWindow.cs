using Assets.Scripts.Tools;
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
        private Action _currentCallback;
        private bool _isVisible;

        private void Awake()
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
            gameObject.SetActive(false);

            _showSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Append(_image.DOFade(Constants.One, _fadeDuration))
                .OnComplete(InvokeCallback)
                .Pause();

            _hideSequence = DOTween.Sequence()
                .Append(_image.DOFade(Constants.Zero, _fadeDuration))
                .OnComplete(() =>
                {
                    InvokeCallback();
                    gameObject.SetActive(false);
                })
                .SetAutoKill(false)
                .Pause();
        }

        public void Show(Action onComplete = null)
        {
            if (_isVisible || _showSequence.IsPlaying())
            {
                return;
            }

            _isVisible = true;
            _currentCallback = onComplete;
            gameObject.SetActive(true);

            DOTween.Kill(this);
            _showSequence.Restart();
        }

        public void Hide(Action onComplete = null)
        {
            if (_isVisible == false || _showSequence.IsPlaying())
            {
                return;
            }

            _isVisible = false;
            _currentCallback = onComplete;

            DOTween.Kill(this);
            _hideSequence.Restart();
        }

        private void InvokeCallback()
        {
            _currentCallback?.Invoke();
            _currentCallback = null;
        }

        private void OnDestroy()
        {
            _showSequence?.Kill();
            _hideSequence?.Kill();
        }
    }
}