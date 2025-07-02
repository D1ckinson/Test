using Assets.Scripts.Characters.EnemyConfigs;
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
        [SerializeField] private EnemyItemDropConfig _dropConfig;

        private Health _health;

        public event Action<Enemy> Died;

        public int ExperienceValue => _dropConfig.ExperienceValue;
        public int CoinDropChance => _dropConfig.CoinDropChance;
        public int CoinValue => _dropConfig.CoinValue;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _dropConfig.ThrowIfNull();
        }
#endif

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
