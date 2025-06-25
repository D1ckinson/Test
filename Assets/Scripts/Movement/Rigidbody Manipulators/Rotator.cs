using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    internal class Rotator
    {
        private readonly Rigidbody _rigidbody;
        private readonly float _speed;

        internal Rotator(Rigidbody rigidbody, float speed)
        {
            rigidbody.ThrowIfNull();
            speed.ThrowIfZeroOrLess();

            _rigidbody = rigidbody;
            _speed = speed;
        }

        internal void Rotate(Vector3 direction)
        {
            direction.ThrowIfNotNormalize();
            direction.y = Constants.Zero;

            Quaternion fromRotation = _rigidbody.rotation;
            Quaternion toRotation = Quaternion.LookRotation(direction);

            if (fromRotation == toRotation)
            {
                return;
            }

            Quaternion rotation = Quaternion.RotateTowards(fromRotation, toRotation, _speed * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(rotation);
        }
    }
}
