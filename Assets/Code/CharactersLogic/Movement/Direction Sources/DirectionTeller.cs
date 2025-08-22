using Assets.Code.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    public class DirectionTeller : MonoBehaviour, ITellDirection
    {
        private const float CalculateDelay = 0.15f;

        private Transform _target;
        private Vector3 _moveDirection;
        private float _time;

        public event Action<Vector3> DirectionChanged;

        public void SetTarget(Transform target)
        {
            _target = target.ThrowIfNull();
            UpdateService.RegisterUpdate(CalculateDirection);
        }

        public void Stop()
        {
            UpdateService.UnregisterUpdate(CalculateDirection);

            _moveDirection = Vector3.zero;
            DirectionChanged?.Invoke(_moveDirection);
        }

        public void Run()
        {
            UpdateService.RegisterUpdate(CalculateDirection);
        }

        private void CalculateDirection()
        {
            if (this.IsActive() == false || _target.IsNull() || _target.IsActive() == false)
            {
                if (_moveDirection != Vector3.zero)
                {
                    _moveDirection = Vector3.zero;
                    DirectionChanged?.Invoke(_moveDirection);
                }

                return;
            }

            _time -= Time.deltaTime;

            if (_time > Constants.Zero)
            {
                return;
            }

            _time = CalculateDelay;
            Vector3 moveDirection = (_target.position - transform.position).normalized;

            if (Mathf.Approximately(moveDirection.x, _moveDirection.x) && Mathf.Approximately(moveDirection.z, _moveDirection.z))
            {
                return;
            }

            _moveDirection = moveDirection;
            DirectionChanged?.Invoke(_moveDirection);
        }
    }
}
