using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Health))]
    internal class HitEffect : MonoBehaviour
    {
        private const float FlashInDuration = 0.05f;
        private const float FlashOutDuration = 0.4f;

        private static readonly int _colorShaderID = Shader.PropertyToID("_Color");//
        private static readonly Color _hitColor = Color.white;//

        [SerializeField] private Renderer _renderer;

        private MaterialPropertyBlock _propertyBlock;
        private Color _originalColor;
        private Health _health;
        private float _timer;
        private bool _isFlashingIn;

        private float CurrentDuration => _isFlashingIn ? FlashInDuration : FlashOutDuration;
        private bool IsTimerExpired => _timer <= Constants.Zero;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _renderer.ThrowIfNull();
        }
#endif

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_propertyBlock);

            _originalColor = _renderer.sharedMaterial.color;

            _health = GetComponent<Health>();
            _health.GetHit += StartEffect;
        }

        private void Update()
        {
            if (IsTimerExpired)
            {
                return;
            }

            _timer -= Time.deltaTime;
            float progress = Constants.One - _timer / CurrentDuration;

            Color color = _isFlashingIn
                ? Color.Lerp(_originalColor, _hitColor, progress)
                : Color.Lerp(_hitColor, _originalColor, progress);

            _propertyBlock.SetColor(_colorShaderID, color);
            _renderer.SetPropertyBlock(_propertyBlock);

            if (IsTimerExpired && _isFlashingIn)
            {
                _isFlashingIn = false;
                _timer = FlashOutDuration;
            }
        }

        private void OnDisable()
        {
            _timer = Constants.Zero;
        }

        private void OnDestroy()
        {
            if (_health != null)
                _health.GetHit -= StartEffect;

            if (_renderer != null)
            {
                _propertyBlock.SetColor(_colorShaderID, _originalColor);
                _renderer.SetPropertyBlock(_propertyBlock);
            }
        }

        private void StartEffect()
        {
            _isFlashingIn = true;
            _timer = FlashInDuration;
        }
    }
}
