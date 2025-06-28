using Assets.Scripts.Combat;
using Assets.Scripts.Movement;
using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Characters
{
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(DirectionTeller))]
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour, IPoolable
    {
        [field: SerializeField][field: Min(10)] public int ExperienceValue { get; private set; } = 10;

        private Health _health;

        public event Action<Enemy> Died;

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.Died -= OnDeath;
            }
        }

        public void Initialize(Transform target)
        {
            GetComponent<DirectionTeller>().Initialize(target);
            GetComponent<CharacterMovement>().Initialize();

            _health = GetComponent<Health>();
            _health.SetMaxValue();
            _health.Died += OnDeath;
        }

        public void Enable()
        {
            _health.SetMaxValue();
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        private void OnDeath()
        {
            Died?.Invoke(this);
        }
    }
}
