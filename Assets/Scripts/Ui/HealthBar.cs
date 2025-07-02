using Assets.Scripts.Combat;
using Assets.Scripts.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ui
{
    internal class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _healthBarFilling;
        [SerializeField] private Health _health;
        [SerializeField] private bool _isFullHealthHide = true;

        private Transform _transform;
        private Camera _camera;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _healthBar.ThrowIfNull();
            _healthBarFilling.ThrowIfNull();
        }
#endif

        private void Awake()
        {
            _health.ThrowIfNull();

            _camera = Camera.main;
            _health.ValueChanged += SetSliderValue;
            _transform = transform;
        }

        private void LateUpdate()
        {
            Vector3 lookPoint = new(_transform.position.x, _camera.transform.position.y, _camera.transform.position.z);
            _transform.LookAt(lookPoint);
        }

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.ValueChanged -= SetSliderValue;
            }
        }

        private void SetSliderValue(float value)
        {
            value.ThrowIfNegative();

            if (_isFullHealthHide && Mathf.Approximately(value, _health.MaxValue))
            {
                _healthBar.gameObject.SetActive(false);
            }
            else if (_healthBarFilling.IsActive() == false)
            {
                _healthBar.gameObject.SetActive(true);
            }

            float percentValue = value / _health.MaxValue;
            _healthBarFilling.fillAmount = percentValue;
        }
    }
}