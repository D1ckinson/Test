using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    internal class Mover
    {
        private readonly Rigidbody _rigidbody;
        private readonly float _speed;

        internal Mover(Rigidbody rigidbody, float speed)
        {
            rigidbody.ThrowIfNull();
            speed.ThrowIfZeroOrLess();

            _rigidbody = rigidbody;
            _speed = speed;
        }

        internal void Move(Vector3 direction)
        {
            direction.ThrowIfNotNormalize();
            direction.y = Constants.Zero;

            Vector3 position = _rigidbody.position + direction * (_speed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(position);
        }
    }
}
