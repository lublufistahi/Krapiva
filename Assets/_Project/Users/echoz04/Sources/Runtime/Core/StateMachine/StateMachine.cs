using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Runtime.Core.StateMachine
{
    public class StateMachine
    {
        protected readonly Dictionary<Type, IState> _states;
        
        protected IState _activeState;

        public StateMachine()
        {
            _states = new Dictionary<Type, IState>(9);
        }

        public void Register(IState state)
        {
            var type = state.GetType();
            
            if (_states.ContainsKey(type) == false)
                _states.Add(type, state);
        }

        public void Enter<TState>() where TState : IState
        {
            var type = typeof(TState);
            
            if (_activeState != null && _activeState.GetType() == type)
                return;

            if (_states.TryGetValue(type, out var newState) == false)
            {
                Debug.LogWarning($"State {type.Name} is not registered.");
                
                return;
            }

            _activeState?.Exit();
            _activeState = newState;
            _activeState.Enter();
        }
    }
}