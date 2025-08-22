﻿using Assets.Code.Tools;
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

            if (type == _currentState?.GetType())
            {
                return;
            }

            _states.TryGetValue(type, out IState state).ThrowIfFalse(new InvalidOperationException());

            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState.Update();
        }
    }
}
