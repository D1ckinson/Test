using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    internal class Mover
    {
        private readonly Rigidbody _rigidbody;

        internal Mover(Rigidbody rigidbody)
        {
            rigidbody.ThrowIfNull();
            _rigidbody = rigidbody;
        }

        internal void AddSlow(float multiplier)
        {
            throw new NotImplementedException();
        }

        internal void Move(Vector3 direction, float speed)
        {
            direction.ThrowIfNotNormalize();
            speed.ThrowIfZeroOrLess();
            direction.y = Constants.Zero;

            Vector3 position = _rigidbody.position + direction * (speed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(position);
        }
    }
}
