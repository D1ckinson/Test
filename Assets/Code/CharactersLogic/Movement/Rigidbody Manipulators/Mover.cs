using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    internal class Mover
    {
        private readonly Rigidbody _rigidbody;
        private readonly List<float> _multipliers;

        private float _maxSpeed;
        private float _currentSpeed;

        internal Mover(Rigidbody rigidbody)
        {
            _rigidbody = rigidbody.ThrowIfNull();
            _multipliers = new();
        }

        public Mover SetSpeed(float speed)
        {
            _maxSpeed = speed.ThrowIfZeroOrLess();
            _currentSpeed = _maxSpeed;

            return this;
        }

        public void AddMaxSpeed(float value)
        {
            _maxSpeed += value.ThrowIfNegative();
            CalculateSpeed();
        }

        internal void AddMultiplier(float multiplier)
        {
            _multipliers.Add(multiplier.ThrowIfZeroOrLess().ThrowIfMoreThan(Constants.One));
            CalculateSpeed();
        }

        internal void Move(Vector3 direction)
        {
            direction.ThrowIfNotNormalize();
            direction.y = Constants.Zero;

            Vector3 position = _rigidbody.position + direction * (_currentSpeed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(position);
        }

        internal void RemoveMultiplier(float multiplier)
        {
            if (_multipliers.Remove(multiplier) == false)
            {
                throw new ArgumentException();
            }
        }

        private void CalculateSpeed()
        {
            float resultMultiplier = Constants.One;
            _multipliers.ForEach(multiplier => resultMultiplier *= multiplier);

            _currentSpeed = _maxSpeed * resultMultiplier;
        }
    }
}
