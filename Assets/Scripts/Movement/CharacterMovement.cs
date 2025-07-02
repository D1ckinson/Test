using Assets.Scripts.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private MovementConfig _config;

        private ITellDirection _directionSource;//
        private Mover _mover;
        private Rotator _rotator;
        private Vector3 _direction;
        private Rigidbody _rigidbody;
        private bool _isSlowed;
        private List<float> _multipliers;
        private float _multiplier;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _config.ThrowIfNull();
        }
#endif

        private void Update()
        {

        }

        private void FixedUpdate()
        {
            if (_direction == Vector3.zero)
            {
                return;
            }

            _mover.Move(_direction, _isSlowed ? _config.CalculateSpeed() : _config.MoveSpeed);
            _rotator.Rotate(_direction);
        }

        public void AddSlow(Slow slow)
        {
            _mover.AddSlow(slow.Multiplier);
            StartCoroutine(SlowProcess(slow));
        }

        private IEnumerator SlowProcess(Slow slow)
        {
            float duration = slow.Duration;

            while (duration > 0)
            {
                duration -= Time.deltaTime;

                yield return null;
            }

            _multipliers.Remove(slow.Multiplier);
            CalculateSlow();
        }

        private void CalculateSlow()
        {
            if (_multipliers.Count > 0)
            {
                _multiplier = _multipliers.Sum();

                return;
            }

            _isSlowed = false;
        }

        public void AddSlow(float duration, float multiplier)
        {

        }


        private void OnDisable()
        {
            if (_rigidbody != null)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }

        private void OnDestroy()
        {
            if (_directionSource != null)
            {
                _directionSource.DirectionChanged -= SetDirection;
            }
        }

        public void AddSlow(float multip, float time)
        {

        }

        public void Initialize()
        {
            if (TryGetComponent(out ITellDirection directionSource) == false)
            {
                throw new MissingComponentException();
            }

            _directionSource = directionSource;
            _directionSource.DirectionChanged += SetDirection;

            _rigidbody = GetComponent<Rigidbody>();

            _mover = new(_rigidbody);
            _rotator = new(_rigidbody, _config.RotationSpeed);
        }

        private void SetDirection(Vector3 vector)
        {
            _direction = vector;
        }
    }

    public struct Slow
    {
        public readonly float Duration;
        public readonly float Multiplier;

        public Slow(float duration, float multiplier)
        {
            duration.ThrowIfZeroOrLess();
            multiplier.ThrowIfZeroOrLess();

            Duration = duration;
            Multiplier = multiplier;
        }
    }
}
