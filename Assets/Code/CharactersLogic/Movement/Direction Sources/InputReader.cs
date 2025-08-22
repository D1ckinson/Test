using Assets.Code.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    public class InputReader : MonoBehaviour, ITellDirection
    {
        private InputControls _inputActions;
        private Vector2 _moveDirection;

        public event Action<Vector3> DirectionChanged;

        private void Awake()
        {
            _inputActions = new();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void Update()
        {
            Vector2 moveDirection = _inputActions.Player.Move.ReadValue<Vector2>();

            if (_moveDirection == moveDirection)
            {
                return;
            }

            _moveDirection = moveDirection;
            DirectionChanged?.Invoke(new(_moveDirection.x, Constants.Zero, _moveDirection.y));
        }

        public void Run()
        {

        }

        public void Stop()
        {

        }
    }
}
