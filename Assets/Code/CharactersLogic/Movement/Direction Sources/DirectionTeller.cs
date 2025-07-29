using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    public class DirectionTeller : MonoBehaviour, ITellDirection
    {
        private Transform _target;
        private Vector3 _moveDirection;

        public event Action<Vector3> DirectionChanged;

        public void SetTarget(Transform target)
        {
            _target = target.ThrowIfNull();
        }

        private void Update()
        {
            if (_target.IsNull() && _target.gameObject.activeSelf)
            {
                if (_moveDirection != Vector3.zero)
                {
                    _moveDirection = Vector3.zero;
                    DirectionChanged?.Invoke(_moveDirection);
                }

                return;
            }

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
