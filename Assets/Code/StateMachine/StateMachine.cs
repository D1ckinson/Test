using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.State_Machine
{
    public class StateMachine
    {
        private readonly Dictionary<Type, IState> _states = new();

        private IState _currentState;

        public StateMachine AddState(IState state)
        {
            _states.Add(state.ThrowIfNull().GetType(), state);

            return this;
        }

        public void SetState<T>() where T : IState
        {
            Type type = typeof(T);

            if (_currentState == null)
            {

            }

            if (type == _currentState?.GetType())
            {
                return;
            }

            _currentState?.Exit();
            _currentState = _states.GetValueOrThrow(type);
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState.Update();
        }
    }
}
