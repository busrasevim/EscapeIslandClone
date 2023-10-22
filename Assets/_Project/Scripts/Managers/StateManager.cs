using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class StateManager<EState> : IInitializable, ITickable where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> CurrentState;

    [Inject] protected SignalBus _signalBus;

    public void Initialize()
    {
        Init();
        SetStates();
    }

    public void Tick()
    {
        CurrentState?.OnUpdate();
    }

    protected void TransitionToState(EState stateKey)
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

    public void NextState()
    {
        if (CurrentState != null && CurrentState.GetNextState() != null)
            TransitionToState(CurrentState.GetNextState());
    }
}