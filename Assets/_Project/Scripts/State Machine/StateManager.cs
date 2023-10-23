using System;
using System.Collections.Generic;
using _Project.Scripts.States.Interfaces;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.State_Machine
{
    public abstract class StateManager<EState> : IInitializable, ITickable where EState : Enum
    {
        protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

        private BaseState<EState> CurrentState;

        [Inject] protected SignalBus _signalBus;

        public void Initialize()
        {
            Init();
            SetStates();
        }

        public void Tick()
        {
            if(CurrentState is ITickableState tickableState)
                tickableState.OnUpdate();
        }

        private void TransitionToState(EState stateKey)
        {
            if (CurrentState != null && CurrentState == States[stateKey])
                return;

            CurrentState?.OnExit();
            CurrentState = States[stateKey];
            CurrentState.OnEnter();

            Debug.Log("The state " + stateKey + typeof(EState) + " starts.");
        }

        protected abstract void SetStates();
        protected abstract void Init();

        public void SetStateWithKey(EState stateKey)
        {
            TransitionToState(stateKey);
        }
    }
}