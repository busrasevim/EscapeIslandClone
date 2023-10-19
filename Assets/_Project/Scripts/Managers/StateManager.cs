using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class StateManager<EState> : IInitializable, ITickable where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> CurrentState;

    protected bool IsTransitioningState = false;

    public void Initialize()
    {
        SetStates();
    }

    public void Tick()
    {
        if (IsTransitioningState || CurrentState==null) return;

        var nextStateKey = CurrentState.GetNextState();

        if (nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.OnUpdate();
        }
        else
        {
            TransitionToState(nextStateKey);
        }
    }

    public void TransitionToState(EState stateKey)
    {
        IsTransitioningState = true;
        CurrentState?.OnExit();
        CurrentState = States[stateKey];
        CurrentState.OnEnter();
        IsTransitioningState = false;
    }

    protected abstract void SetStates();

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