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
        private readonly float _defaultSpeed;

        private float _speed;

        internal Mover(Rigidbody rigidbody, float speed)
        {
            _rigidbody = rigidbody.ThrowIfNull();
            _defaultSpeed = speed.ThrowIfZeroOrLess();
            _speed = _defaultSpeed;
            _multipliers = new();
        }

        internal void AddMultiplier(float multiplier)
        {
            _multipliers.Add(multiplier.ThrowIfZeroOrLess().ThrowIfMoreThan(Constants.One));
        }

        internal void Move(Vector3 direction)
        {
            direction.ThrowIfNotNormalize();
            direction.y = Constants.Zero;

            Vector3 position = _rigidbody.position + direction * (_speed * Time.fixedDeltaTime);
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

            _speed = _defaultSpeed * resultMultiplier;
        }
    }
}
