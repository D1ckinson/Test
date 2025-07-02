using Assets.Scripts.Combat;
using Assets.Scripts.Movement;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(Health))]
    public class Hero : MonoBehaviour
    {
        private Health _health;

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.Died -= OnDied;
            }
        }

        public void Initialize()
        {
            GetComponent<CharacterMovement>().Initialize();
            _health = GetComponent<Health>();
            _health.SetMaxValue();
            _health.Died += OnDied;
        }

        private void OnDied()
        {
            gameObject.SetActive(false);
        }
    }
}
