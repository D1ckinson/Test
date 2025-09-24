﻿using Assets.Code.CharactersLogic.Movement.Direction_Sources.Interfaces;
using Assets.Code.Tools;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Code.CharactersLogic.Movement.Direction_Sources
{
    internal class DirectionTellerFrom : ITargetDirectionTeller
    {
        private const float CalculateDelay = 0.15f;
        private const float SqrMoveRange = 180;

        private readonly Transform _owner;
        private Transform _target;
        private Vector3 _moveDirection;
        private float _time;

        public DirectionTellerFrom(Transform owner)
        {
            _owner = owner.ThrowIfNull();
        }

        public event Action<Vector3> DirectionChanged;

        public void SetTarget(Transform target)
        {
            _target = target.ThrowIfNull();
        }

        public void Enable()
        {
            UpdateService.RegisterUpdate(CalculateDirection);
        }

        public void Disable()
        {
            UpdateService.UnregisterUpdate(CalculateDirection);

            _moveDirection = Vector3.zero;
            _time = CalculateDelay;
            DirectionChanged?.Invoke(_moveDirection);
        }

        private void CalculateDirection()
        {
            if (_owner.IsActive() == false || _target.IsNull() || _target.IsActive() == false)
            {
                Disable();

                return;
            }

            _time -= Time.deltaTime;

            if (_time > Constants.Zero)
            {
                return;
            }

            _time = CalculateDelay;

            if ((_owner.position - _target.position).sqrMagnitude > SqrMoveRange)
            {
                _moveDirection = Vector3.zero;
                DirectionChanged?.Invoke(_moveDirection);

                return;
            }

            Vector3 moveDirection = (_owner.position - _target.position).normalized;

            if (_moveDirection.Compare(moveDirection, Constants.CompareAccuracy))
            {
                return;
            }

            _moveDirection = moveDirection;
            DirectionChanged?.Invoke(_moveDirection);
        }
    }
}
