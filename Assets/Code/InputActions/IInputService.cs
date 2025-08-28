using Assets.Scripts.Movement;
using System;

namespace Assets.Code.InputActions
{
    public interface IInputService : ITellDirection
    {
        public event Action PausePressed;
    }
}
