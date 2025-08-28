using Assets.Code.InputActions;
using Assets.Code.Tools;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Movement
{
    public class InputReader : IInputService
    {
        private readonly InputControls _inputControls;

        private Vector2 _previousDirection;

        public InputReader()
        {
            _inputControls = new InputControls();
            _inputControls.Player.Move.performed += OnMovePerformed;
            _inputControls.Player.Move.canceled += OnMoveCanceled;
            //_inputControls.Player.Pause.performed += OnPausePerformed;
        }

        ~InputReader()
        {
            _inputControls.Disable();
            _inputControls.Dispose();
        }

        public event Action<Vector3> DirectionChanged;
        public event Action PausePressed;

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();

            if (direction.Compare(_previousDirection, Constants.CompareAccuracy))
            {
                return;
            }

            _previousDirection = direction;
            DirectionChanged?.Invoke(new Vector3(direction.x, Constants.Zero, direction.y));
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _previousDirection = Vector2.zero;
            DirectionChanged?.Invoke(_previousDirection);
        }

        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            PausePressed?.Invoke();
        }

        public void Enable()
        {
            _inputControls.Enable();
        }

        public void Disable()
        {
            _inputControls.Disable();
        }
    }
}
