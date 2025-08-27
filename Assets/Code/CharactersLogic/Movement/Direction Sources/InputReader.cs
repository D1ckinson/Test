using Assets.Code.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    public class InputReader : ITellDirection
    {
        private InputControls _inputActions;
        private Vector2 _moveDirection;

        public event Action<Vector3> DirectionChanged;

        public InputReader(InputControls inputs)
        {
            _inputActions = inputs.ThrowIfNull();
        }

        private void ReadInput()
        {
            Vector2 moveDirection = _inputActions.Player.Move.ReadValue<Vector2>();

            if (_moveDirection.Compare(moveDirection,Constants.CompareAccuracy))
            {
                return;
            }

            _moveDirection = moveDirection;
            DirectionChanged?.Invoke(new(_moveDirection.x, Constants.Zero, _moveDirection.y));
        }

        public void Run()
        {
            _inputActions.Enable();
            UpdateService.RegisterUpdate(ReadInput);
        }

        public void Stop()
        {
            _inputActions.Disable();
            UpdateService.UnregisterUpdate(ReadInput);
        }
    }
}
