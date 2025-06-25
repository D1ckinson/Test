using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private MovementConfig _config;

        private ITellDirection _directionSource;
        private Mover _mover;
        private Rotator _rotator;
        private Vector3 _direction;
        private Rigidbody _rigidbody;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _config.ThrowIfNull();
        }
#endif

        private void FixedUpdate()
        {
            if (_direction == Vector3.zero)
            {
                return;
            }

            _mover.Move(_direction);
            _rotator.Rotate(_direction);
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

        public void Initialize()
        {
            _directionSource = GetComponent<ITellDirection>();
            _directionSource.ThrowIfNull();
            _directionSource.DirectionChanged += SetDirection;

            _rigidbody = GetComponent<Rigidbody>();

            _mover = new(_rigidbody, _config.MoveSpeed);
            _rotator = new(_rigidbody, _config.RotationSpeed);
        }

        private void SetDirection(Vector3 vector)
        {
            _direction = vector;
        }
    }
}
